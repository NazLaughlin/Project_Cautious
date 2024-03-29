﻿using System;
using System.Collections.Generic;
using Project_Cautious;
using Project_Cautious.Cast;
using Project_Cautious.Cast.Basics;
using Project_Cautious.Script;
using Project_Cautious.Services;
using Project_Cautious.UI;

namespace Project_Cautious
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create the cast
            Dictionary<string, List<Actor>> cast = new Dictionary<string, List<Actor>>();

            Dictionary<string, List<Actor>> menu = new Dictionary<string, List<Actor>>();
            menu["main"] = new List<Actor>();
            Banner title = new Banner(true, Constants.BACKGROUND_TITLE, "");
            title.SetWidth(Constants.MAX_X);
            title.SetHeight(Constants.MAX_Y);
            title.SetPosition(new Point(0,0));
            menu["main"].Add(title);

            cast["background"] = new List<Actor>();
            Banner background = new Banner(true, Constants.BACKGROUND_MAIN, "");
            background.SetWidth(Constants.MAX_X);
            background.SetHeight(Constants.MAX_Y);
            background.SetPosition(new Point(0,0));
            cast["background"].Add(background);
            cast["player"] = new List<Actor>();
            Player player = new Player();
            PlayerOverlay playerOverlay = new PlayerOverlay(player);
            cast["player"].Add(playerOverlay);
            cast["player"].Add(player);
            
            cast["enemies"] = new List<Actor>();
            cast["bullets"] = new List<Actor>();

            cast["banners"] = new List<Actor>();
            Banner endBanner = new Banner(false, Constants.GAME_OVER_IMAGE,"Press Enter to Exit");
            cast["banners"].Add(endBanner);

            cast["counters"] = new List<Actor>();
            Counter waveCounter = new Counter("Wave",0,new Point(0,0));
            cast["counters"].Add(waveCounter);
            Counter lifeCounter = new Counter("Health",player.GetHealth(), new Point(0,0));
            lifeCounter.SetPosition(new Point(Constants.MAX_X - lifeCounter.GetWidth(), 0));
            cast["counters"].Add(lifeCounter);

            // Create the script
            Dictionary<string, List<Action>> script = new Dictionary<string, List<Action>>();

            OutputService outputService = new OutputService();
            InputService inputService = new InputService();
            PhysicsService physicsService = new PhysicsService();
            AudioService audioService = new AudioService();

            script["main"] = new List<Action>();
            script["output"] = new List<Action>();
            script["input"] = new List<Action>();
            script["update"] = new List<Action>();
            script["end"] = new List<Action>();
            script["endInput"] = new List<Action>();

            DrawActorsAction drawActorsAction = new DrawActorsAction(outputService);
            script["output"].Add(drawActorsAction);
            script["main"].Add(drawActorsAction);
            BGMAction bgmAction = new BGMAction(audioService);
            script["output"].Add(bgmAction);
            script["main"].Add(bgmAction);
            // TODO: Add additional actions here to handle the input, move the actors, handle collisions, etc.
            ControlActorsAction controlActorsAction = new ControlActorsAction(inputService);
            script["input"].Add(controlActorsAction);
            
            MoveActorsAction moveActorsAction = new MoveActorsAction();
            script["update"].Add(moveActorsAction);
            HandleCollisionsAction handleCollisionsAction = new HandleCollisionsAction(physicsService, audioService);
            script["update"].Add(handleCollisionsAction);
            HandleOffScreenAction handleOffScreenAction = new HandleOffScreenAction();
            script["update"].Add(handleOffScreenAction);
            RemoveDeadAction RemoveDeadAction = new RemoveDeadAction(audioService);
            script["update"].Add(RemoveDeadAction);
            AttackAction attackAction = new AttackAction(inputService);
            script["update"].Add(attackAction);
            SpawnEnemiesAction spawnEnemiesAction = new SpawnEnemiesAction();
            script["update"].Add(spawnEnemiesAction);
            UpdatePlayerOverlay updatePlayerOverlay = new UpdatePlayerOverlay();
            script["update"].Add(updatePlayerOverlay);

            EndGameAction endGameAction = new EndGameAction(audioService);
            script["end"].Add(endGameAction);

            PlayAgainInputAction playAgainInputAction = new PlayAgainInputAction(inputService);
            script["endInput"].Add(playAgainInputAction);

            // Start up the game
            outputService.OpenWindow(Constants.MAX_X, Constants.MAX_Y, "Project Cautious", Constants.FRAME_RATE);
            audioService.StartAudio();

            audioService.SwitchBGM(Constants.MAIN_BGM);
            while(!inputService.IsContinuePressed()){
                foreach (Action action in script["main"])
                {
                    action.Execute(menu);
                }
            }
            audioService.PlaySound(Constants.SOUND_START);

            Director theDirector = new Director(cast, script);
            audioService.SwitchBGM(Constants.GAME_BGM);
            theDirector.Direct();

            audioService.StopAudio();
        }
    }
}
