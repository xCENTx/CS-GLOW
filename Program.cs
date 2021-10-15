using System;
using helpers.CS;
using System.Threading;
using System.Diagnostics;

namespace CS_GLOW
{
    class Program
    {
        //menu
        static bool HACK = false;
        static bool bHACK = false;
        static bool bRAPID = false;
        static bool bESP = false;
        static string sESP = " ";
        static string sRAPID = " ";

        //mem
        static int i;
        static int client;
        static int engine;
        static int localPlayer;
        static int glowind;
        static int entityBase;
        static int entityTeam;
        static int entityHp;
        static int dormant;
        static int entityList;
        static int team;
        static int _maxPlayers;

        static void Main()
        {
            //Establish Connection to Process
            Console.Title = "SWEDISH GL:OW";
            Console.SetWindowSize(30, 12);
            Console.WriteLine("Searching for process . . .");
            start: Process[] csgo = Process.GetProcessesByName("csgo");
            if (csgo.Length > 0)
            {
                func._menu(sESP, sRAPID);
                mem.ProcessHandle = mem.OpenProcess(0x0008 | 0x0010 | 0x0020, false, csgo[0].Id);
                client = tools.GetModuleBaseAddress(csgo[0], "client.dll");
                engine = tools.GetModuleBaseAddress(csgo[0], "engine.dll");
                Thread RAPID_FIRE = new Thread(_rapidFire) { IsBackground = true }; //asynchronous??
                RAPID_FIRE.Start();
                HACK = true;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Waiting for csgo to start...");
                while (true)
                {
                    Process[] csgo_failed = Process.GetProcessesByName("csgo");
                    if (csgo_failed.Length > 0)
                    {
                        Thread.Sleep(3000); //Wait for client.dll & engine.dll to be loaded into csgo.
                        goto start;
                    }
                    Thread.Sleep(5000);
                }
            }

            while (HACK)
            {
                //Establish Keybinds
                short keyEND = func.GetAsyncKeyState(func.VK_END);
                short keyNUM1 = func.GetAsyncKeyState(func.VK_NUMPAD1);
                short keyNUM2 = func.GetAsyncKeyState(func.VK_NUMPAD2);
                short keySHIFT = func.GetAsyncKeyState(func.VK_LSHIFT);

                if ((keyNUM1 & 1) == 1)
                {
                    bESP = !bESP;

                    if (bESP)
                    {
                        sESP = "X";
                        func._menu(sESP, sRAPID);
                        bHACK = true;
                    }
                    else
                    {
                        sESP = " ";
                        func._menu(sESP, sRAPID);
                        bHACK = false;
                    }
                }

                if ((keyNUM2 & 1) == 1)
                {
                    bRAPID = !bRAPID;

                    if (bRAPID)
                    {
                        sRAPID = "X";
                        func._menu(sESP, sRAPID);
                    }
                    else
                    {
                        sRAPID = " ";
                        func._menu(sESP, sRAPID);
                    }
                }

                //QUIT
                if ((keyEND & 1) == 1)
                {
                    HACK = false;
                    break;
                }

                if (bHACK)
                {
                    //is player in game?
                    _status();

                    if (_maxPlayers < 1)
                    {
                        Console.Clear();
                        Console.WriteLine("Not currently in a game . . .");
                        Console.WriteLine("Returning to main");
                        Thread.Sleep(5000);

                        //Disable Hack
                        if (bESP)
                        {
                            sESP = " ";
                            bESP = false;
                            bHACK = false;
                        }
                        if (bRAPID)
                        {
                            sRAPID = " ";
                            bRAPID = false;
                        }
                        func._menu(sESP, sRAPID);
                        continue;
                    }

                    //Entity Loop
                    for (i = 0; i < _maxPlayers; i++)
                    {
                        readvalues();
                        if (entityBase == 0)
                            continue;
                        if (dormant == 1)
                            continue;
                        if (entityHp < 1)
                            continue;

                        if (team != entityTeam)
                        {
                            writeglow(glowind, 1, 0, 0, 1);
                        }
                        else
                        {
                            writeglow(glowind, 0, 1, 0, 1);
                        }
                    }
                }

                Thread.Sleep(1);
            }
        }

        private static void _rapidFire(object obj)
        {
            while (true)
            {
                if (bRAPID)
                {
                    //Mouse Button 3
                    if (func.GetAsyncKeyState(func.VK_XBUTTON1) < 0)
                    {
                        func._rFIRE(65);
                    }
                }
            }
        }

        public static void _status()
        {
            int clientState = mem.ReadMemory<int>(engine + offsets.ClientState);
            _maxPlayers = mem.ReadMemory<int>(clientState + offsets.ClientState_MaxPlayer);
        }

        public static void writeglow(int glw, float red, float green, float blue, float alpha)
        {
            int glowObject = mem.ReadMemory<int>(client + offsets.GlowObjectManager);
            int cEntityGlowOffset = glowObject + (glw * 0x38);
            mem.WriteMemory<float>(cEntityGlowOffset + 0x8, red);
            mem.WriteMemory<float>(cEntityGlowOffset + 0xC, green);
            mem.WriteMemory<float>(cEntityGlowOffset + 0x10, blue);
            mem.WriteMemory<float>(cEntityGlowOffset + 0x14, alpha);
            mem.WriteMemory<int>(cEntityGlowOffset + 0x28, 1); //Enemy Damage Indicator
        }

        public static void readvalues()
        {
            entityList = offsets.EntityList + i * 0x10;
            localPlayer = mem.ReadMemory<int>(client + offsets.LocalPlayer);
            team = mem.ReadMemory<int>(localPlayer + offsets.TeamNum);
            entityBase = mem.ReadMemory<int>(client + entityList);
            entityTeam = mem.ReadMemory<int>(entityBase + offsets.TeamNum);
            entityHp = mem.ReadMemory<int>(entityBase + offsets.health);
            glowind = mem.ReadMemory<int>(entityBase + offsets.GlowIndex);
            dormant = mem.ReadMemory<int>(entityBase + offsets.Dormant);
        }
    }
}