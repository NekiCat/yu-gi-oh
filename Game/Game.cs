using NLua;
using NLua.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TigeR.YuGiOh.Core.Players;

namespace TigeR.YuGiOh.Core
{
    public class Game : IDisposable
    {
        private Dictionary<String, List<LuaFunction>> eventHandler = new Dictionary<String, List<LuaFunction>>();

        private readonly Lua context = new Lua();

        /// <summary>
        /// The player.
        /// </summary>
        public Player Player { get; private set; }

        /// <summary>
        /// The opponent.
        /// </summary>
        public Player Enemy { get; private set; }

        /// <summary>
        /// The current player. One of <see cref="Player"/> or <see cref="Enemy"/>.
        /// </summary>
        public Player TurnPlayer { get; private set; }

        /// <summary>
        /// The class that takes console output and renders it for a certain target, maybe the console or a chat window.
        /// </summary>
        public IMessageLogger MessageLogger { get; }

        public Game(Player player, Player enemy, IMessageLogger logger)
        {
            Player = player;
            Enemy = enemy;
            MessageLogger = logger;

            // Redefine import function to disable all lua imports,
            // practically sandboxing the lua scripts
            context.DoString("import = function () assert (false, \"illegal import\") end");
            
            // Add some functions for the game logic
            context.RegisterFunction("addEventHandler", this, GetType().GetMethod(nameof(AddEventHandler)));
            context.RegisterFunction("triggerEvent", this, GetType().GetMethod(nameof(TriggerEvent)));
            context.RegisterFunction("print", MessageLogger, MessageLogger.GetType().GetMethod(nameof(IMessageLogger.Print)));
            context.RegisterFunction("debug", MessageLogger, MessageLogger.GetType().GetMethod(nameof(IMessageLogger.Debug)));

            // note: lua has an own assert function that throws an exception instead of continuing normally.
            //context.RegisterFunction("assert", MessageLogger, MessageLogger.GetType().GetMethod(nameof(IMessageLogger.Assert)));

            // Load the main game logic into the lua engine.
            context.DoFile(Path.Combine(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), "game.lua"));
        }

        /// <summary>
        /// Runs the game loop. This loop returns once the game finishes.
        /// </summary>
        /// <param name="playerFirst">Whether the player should have the first turn. Else the enemy begins.</param>
        /// <returns></returns>
        public async Task RunAsync(bool playerFirst = false)
        {
            TurnPlayer = playerFirst ? Player : Enemy;
            TriggerEvent("first-turn", new EventArgs());

            int guard = 0;
            while (Player.LifePoints > 0 && Enemy.LifePoints > 0)
            {
                TurnPlayer = TurnPlayer.Equals(Player) ? Enemy : Player;
                context["TurnPlayer"] = TurnPlayer;

                TriggerEvent("turn", new EventArgs());
                
                if (guard++ > 3) return;
            }
        }

        /// <summary>
        /// Registers an event handler. This method is visible from lua.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="func"></param>
        public void AddEventHandler(string name, LuaFunction func)
        {
            if (!eventHandler.ContainsKey(name))
            {
                eventHandler[name] = new List<LuaFunction>();
            }

            eventHandler[name].Add(func);
        }

        /// <summary>
        /// Triggers a specific event. Will trigger all registered events for that name. If
        /// an event throws an error, it will get logged and the next handler gets executed.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="e"></param>
        public void TriggerEvent(string name, EventArgs e = null)
        {
            MessageLogger.SystemDebug("event {0} (has {1} handler)", name, eventHandler.ContainsKey(name) ? eventHandler[name].Count.ToString() : "no");
            if (eventHandler.ContainsKey(name))
            {
                foreach (var func in eventHandler[name])
                {
                    try
                    {
                        func.Call(e ?? new EventArgs());
                    }
                    catch (LuaException ex)
                    {
                        MessageLogger.PrintError(ex);
                        MessageLogger.SystemDebug("Event handler had a failure: Continuing with next handler");
                    }
                }
            }
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
