using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using System.Text.Json;
using TextRPG;

namespace TextRPG
{
     public static class GameSystem
    {
        #region 실패&색상 출력 함수

        public static void FaileInput()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ 잘못된 입력입니다. 다시 시도해주세요.\n");
            Console.ResetColor();
        }
        public static void StringPrintRed(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public static void StringPrintGreen(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        #endregion


        #region 플레이어 함수
        public static void PlayerInfo_Color()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║             📋 캐릭터 상태 보기            ║");
            Console.WriteLine("╚════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"👤 이름       : {Program.player.Name}");
            Console.WriteLine($"💼 직업       : {Program.player.Job}");
            Console.WriteLine($"📈 레벨       : Lv. {Program.player.Level} (Exp: {Program.player.Exp}/{Program.player.ExpToNextLevel})"); // ✅ 경험치 출력
            Console.WriteLine($"❤️ 체력       : {Program.player.HP}");

            int bonusAtk = Program.player.Attack - Program.player.BaseAttack;
            int bonusDef = Program.player.Defense - Program.player.BaseDefense;

            Console.Write("⚔️ 공격력     : ");
            Console.Write($"{Program.player.Attack}");
            if (bonusAtk > 0)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($" (+{bonusAtk})");
                Console.ResetColor();
            }
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("🛡️ 방어력     : ");
            Console.Write($"{Program.player.Defense}");
            if (bonusDef > 0)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($" (+{bonusDef})");
                Console.ResetColor();
            }
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"💰 소지 Gold  : {Program.player.Gold} G");
            Console.ResetColor();
        }

        public static void CheckDeath(Player player)
        {
            if (player.HP > 0)
                return;

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n☠️ 당신은 쓰러졌습니다...");
            Console.ResetColor();

            Thread.Sleep(1000);

            // 골드 차감
            if (player.Gold >= 1000)
            {
                player.Gold -= 1000;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("💰 부활 비용 1000G가 차감되었습니다.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("💰 소지금이 부족하여 부활 비용이 차감되지 않았습니다.");
            }

            // 체력 회복
            player.HP = 50;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("❤️ 체력이 50으로 회복되었습니다.");
            Console.ResetColor();

            Thread.Sleep(1000);
            Console.WriteLine("\n🏰 마을로 돌아갑니다...");
            Thread.Sleep(1000);

            Program.StartGame();
        }

        #endregion


        #region 던전 함수

        // 던전 화면 UI 출력 함수
        public static void DungeonScreenUI()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                                                            ║");
            Console.WriteLine("║     ███████╗ ██╗   ██╗███╗   ██╗ ██████╗  ███████╗ ██████╗ ███╗   ██╗      ║");
            Console.WriteLine("║     ██╔═══██╗██║   ██║████╗  ██║██╔════╝  ██╔════╝██╔═══██╗████╗  ██║      ║");
            Console.WriteLine("║     ██║   ██║██║   ██║██╔██╗ ██║██║  ███╗ █████╗  ██║   ██║██╔██╗ ██║      ║");
            Console.WriteLine("║     ██║   ██║██║   ██║██║╚██╗██║██║   ██║ ██╔══╝  ██║   ██║██║╚██╗██║      ║");
            Console.WriteLine("║     ███████╔╝╚██████╔╝██║ ╚████║╚██████╔╝ ███████╗╚██████╔╝██║ ╚████║      ║");
            Console.WriteLine("║     ╚══════╝  ╚═════╝ ╚═╝  ╚═══╝ ╚═════╝  ╚══════╝ ╚═════╝ ╚═╝  ╚═══╝      ║");
            Console.WriteLine("║                                                                            ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        // 던전 선택 화면 UI 출력 함수
        public static void DungeonScreen()
        {
            DungeonScreenUI();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n⚔️ 입장할 던전을 선택하세요\n");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("1. 🟢 쉬운 던전    (권장 공격력: 10 / 방어력: 5)");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("2. 🟡 일반 던전    (권장 공격력: 45 / 방어력: 40)");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("3. 🔴 어려운 던전  (권장 공격력: 75 / 방어력: 60)");
            Console.ResetColor();
        }

        // 던전 입장 함수
        public static void EnterDungeon(int requiredAtk, int requiredDef, int baseReward)
        {
            var player = Program.player;
            Console.Clear();
            Console.WriteLine("던전 입장 중...");
            Thread.Sleep(1000);

            // 🌀 공통 애니메이션 한 번만 사용
            ShowDungeonBattleAnimation();

            bool isSuccess = true;
            Random rand = new Random();

            if (player.Defense < requiredDef)
            {
                isSuccess = rand.Next(100) >= 40;
            }

            if (!isSuccess)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n❌ 던전 실패! 체력 절반 감소. 보상 없음.");
                Console.ResetColor();
                player.HP /= 2;
                // ✅ 체력 감소 후 죽음 확인
                GameSystem.CheckDeath(player);
            }
            else
            {
                int defDiff = requiredDef - player.Defense;
                int hpLoss = rand.Next(20 + defDiff, 36 + defDiff);
                player.HP -= Math.Max(hpLoss, 1);
                // ✅ 체력 감소 후 죽음 확인
                GameSystem.CheckDeath(player);

                int atk = player.Attack;
                int bonusPercent = rand.Next(atk, atk * 2 + 1);
                int bonusGold = baseReward * bonusPercent / 100;
                int totalGold = baseReward + bonusGold;

                player.Gold += totalGold;

                // ✅ 경험치 지급
                int gainedExp = baseReward / 10;
                player.GainExp(gainedExp);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n✅ 던전 클리어! 보상: {totalGold}G");
                Console.WriteLine($"🩸 잃은 체력: {hpLoss} | 현재 체력: {player.HP}");
                Console.ResetColor();

                BattleSystem.dungeonClearCount++;
            }

            Console.WriteLine("\n아무 키나 누르면 계속합니다...");
            Console.ReadKey();
            BattleSystem.ShowDungeonMenu();
        }



        // 던전 3 UI 출력 함수
        public static void ShowDungeon3UI()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;

            string[] artLines = {
        @"                ,'\   |\",
        @"               / /.:  ;;",
        @"              / :'|| //",
        @"             (| | ||;'",
        @"             / ||,;'-.._",
        @"            : ,;,`';:.--`",
        @"            |:|'`-(\\",
        @"            ::: \-'\`'",
        @"             \\\ \,-`.",
        @"              `'\ `.,-`-._      ,-._",
        @"       ,-.       \  `.,-' `-.  / ,..`.",
        @"      / ,.`.      `.  \ _.-' \',: ``\ \",
        @"     / / :..`-'''``-)  `.   _.:''  ''\ \",
        @"    : :  '' `-..''`/    |-''  |''  '' \ \",
        @"    | |  ''   ''  :     |__..-;''  ''  : :",
        @"    | |  ''   ''  |     ;    / ''  ''  | |",
        @"    | |  ''   ''  ;    /--../_ ''_ '' _| |",
        @"    : :  ''  _;:_/    :._  /-.'',-.'',-. |",
        @"    \ \  '',;'`;/     |_ ,(   `'   `'   \|",
        @"     \ \  \(   /\     :,'  \\",
        @"      \ \.'/  : /    ,)    /",
        @"       \ ':   ':    / \   :",
        @"        `.\    :   :\  \  |",
        @"                \  | `. \ |..-_",
        @"             SSt ) |.  `/___-.-`",
        @"               ,'  -.'.  `. `'        _,)",
        @"               \'\(`.\ `._ `-..___..-','",
        @"                  `'      ``-..___..-'"
    };

            Console.WriteLine("╔════════════════════════════════════════════════════╗");
            foreach (string line in artLines)
            {
                Console.WriteLine($"║ {line.PadRight(50)} ║");
            }
            Console.WriteLine("╚════════════════════════════════════════════════════╝");

            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n🧟 [던전 3: 죽음의 계곡]");
            int winRate = GameSystem.CalculateWinRate(Program.player, 75, 60);
            Console.WriteLine($"📊 권장 공격력: 75 / 방어력: 60");
            Console.WriteLine($"🎯 예상 승리 확률: {winRate}%");
            Console.WriteLine("⚔️ 전투를 시작하시겠습니까?");
            Console.WriteLine("1. ✅ 예   2. ❌ 아니오");
            Console.ResetColor();
        }


        // 던전 2 UI 출력 함수
        public static void ShowDungeon2UI()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;

            string[] artLines = {
        @"           """"---==____   ____==---""""",
        @"        """"""---==='__  """"  __`===---""""""",
        @"         """"""--===(___=-_-=___)===--""""""",
        @"         """"""--=== ) _=====_ ( ===--""""""",
        @"         """"--===//\""""\""/\\===--""""""",
        @"   ___----______---|___-----___|---______-----___",
        @" ,'        """"--==`\`       '/'==--\""""       __`----__",
        @" \          """"---==| \   / |==---\""""  __--""  """"-_",
        @"  \                  `:-| |-:'      \ /'              `\",
        @"   )                 | `/ \' |      /'     ,------_      `\",
        @"  '                  | `-^-' |    /'     /'        `\      \",
        @"                    |       |   |     /\\           \      \",
        @"                    |       |  |     |  \ \          \      \",
        @"                    \       \  |     |___) )          |      |",
        @"                    \       \-""|     |_---'          |      |",
        @"                    _\       \-\     \              /       |",
        @"                  /' \       \  \     \         _,-""       /",
        @"                /   _-\       \__\_____\____--""         /",
        @"               (   ""--\                               /'",
        @"                `-__    \_                         _,-'",
        @"                    `--_  ""-___________________--""",
        @"                        `\   \__    )    )",
        @"                          \     ""--""    /",
        @"                           \__        /'",
        @"                              ""---"""
    };

            Console.WriteLine("╔═════════════════════════════════════════════════════════════════════╗");
            foreach (string line in artLines)
            {
                Console.WriteLine($"║ {line.PadRight(67)} ║");
            }
            Console.WriteLine("╚═════════════════════════════════════════════════════════════════════╝");

            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n🌋 [던전 2: 용암 골짜기]");
            int winRate = GameSystem.CalculateWinRate(Program.player, 45, 40);
            Console.WriteLine($"📊 권장 공격력: 45 / 방어력: 40");
            Console.WriteLine($"🎯 예상 승리 확률: {winRate}%");
            Console.WriteLine("⚔️ 전투를 시작하시겠습니까?");
            Console.WriteLine("1. ✅ 예   2. ❌ 아니오");
            Console.ResetColor();
        }

        // 던전 1 UI 출력 함수
        public static void ShowDungeon1UI()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            string[] artLines = {
@" _________________________________________________________",
@"/|     -_-                                             _-  |\",
@"/ |_-_- _                                         -_- _-   -| \   ",
@"  |                            _-  _--                      | ",
@"  |                            ,                            |",
@"  |      .-'````````'.        '(`        .-'```````'-.      |",
@"  |    .` |           `.      `)'      .` |           `.    |",
@"  |   /   |   ()        \      U      /   |    ()       \   |",
@"  |  |    |    ;         | o   T   o |    |    ;         |  |",
@"  |  |    |     ;        |  .  |  .  |    |    ;         |  |",
@"  |  |    |     ;        |   . | .   |    |    ;         |  |",
@"  |  |    |     ;        |    .|.    |    |    ;         |  |",
@"  |  |    |____;_________|     |     |    |____;_________|  |",
@"  |  |   /  __ ;   -     |     !     |   /     `'() _ -  |  |",
@"  |  |  / __  ()        -|        -  |  /  __--      -   |  |",
@"  |  | /        __-- _   |   _- _ -  | /        __--_    |  |",
@"  |__|/__________________|___________|/__________________|__|",
@"/                                             _ -        lc \",
@"/   -_- _ -             _- _---                       -_-  -_ \"
    };

            // 박스 상단
            Console.WriteLine("╔═════════════════════════════════════════════════════════════════════╗");

            // 아트 삽입 (좌우 여백 3칸 삽입해서 박스에 정렬)
            foreach (string line in artLines)
            {
                Console.WriteLine($"║ {line.PadRight(67)} ║");
            }

            // 박스 하단
            Console.WriteLine("╚═════════════════════════════════════════════════════════════════════╝");

            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n🦖 [던전 1: 해골의 던전]");
            int winRate = GameSystem.CalculateWinRate(Program.player, 10, 5);
            Console.WriteLine($"📊 예상 승리 확률: {winRate}%");
            Console.WriteLine("⚔️ 전투를 시작하시겠습니까?");
            Console.WriteLine("1. ✅ 예   2. ❌ 아니오");
            Console.ResetColor();
        }



        // 던전 화면 출력 함수
        public static void ShowDungeonScene(Action uiAction, Action enterAction, Action returnAction)
        {
            Console.Clear();
            uiAction?.Invoke(); // 던전 UI 출력

            Console.Write("\n>> ");
            string input = Console.ReadLine();

            if (input == "1")
            {
                enterAction?.Invoke(); // 전투 시작
            }
            else
            {
                returnAction?.Invoke(); // 던전 메뉴로 돌아가기
            }
        }
        // 던전 선택 화면 함수들
        public static void ShowDungeon1Scene()
        {
            GameSystem.ShowDungeonScene(
                ShowDungeon1UI,
                () => EnterDungeon(10, 5, 1000),
                BattleSystem.ShowDungeonMenu
            );
        }

        public static void ShowDungeon2Scene()
        {
            GameSystem.ShowDungeonScene(
                ShowDungeon2UI,
                () => EnterDungeon(45, 40, 1700),
                BattleSystem.ShowDungeonMenu
            );
        }

        public static void ShowDungeon3Scene()
        {
            GameSystem.ShowDungeonScene(
                ShowDungeon3UI,
                () => EnterDungeon(75, 60, 2500),
                BattleSystem.ShowDungeonMenu
            );
        }
        // 던전 성공확률 계산 함수
        public static int CalculateWinRate(Player player, int requiredAtk, int requiredDef)
        {
            double atkRatio = (double)player.Attack / requiredAtk;
            double defRatio = (double)player.Defense / requiredDef;

            // 평균 내고 퍼센트로 환산
            double avgRatio = (atkRatio + defRatio) / 2.0;
            int winRate = (int)(avgRatio * 100);

            // 제한 범위 설정
            if (winRate > 95) winRate = 95;
            if (winRate < 20) winRate = 20;

            return winRate;
        }
        // 레벨업 애니메이션 함수
        public static void ShowLevelUpAnimation(Player player)
        {
            Console.Clear();
            string[] frames =
            {
        "\n\n\n\n            🎉",
        "\n\n\n      🎉     🎉",
        "\n\n   🎉   🎉   🎉",
        "\n🎉 🎉 LEVEL UP! 🎉 🎉",
        $"         Lv. {player.Level} 도달!",
    };

            foreach (string frame in frames)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(frame);
                Console.ResetColor();
                Thread.Sleep(300);  // 프레임 간 딜레이
            }

            Thread.Sleep(800);  // 마지막 프레임 더 오래 보여주기
            Console.Clear();
        }

        public static void ShowDungeonBattleAnimation()
        {
            string[] enemyFrames =
            {
        "             🦖",
        "          🦖",
        "       🦖",
        "    🦖",
        "       🦖",
        "          🦖",
        "             🦖",
    };

            string[] battleActions = { "👊 공격함!", "💨 회피함!", "🛡️ 방어 성공!", "⚔️ 강타!", "❌ 빗맞음!" };
            Random rand = new Random();

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\n⚔️ 전투 시작!\n");
            Console.ResetColor();
            Thread.Sleep(800);

            // 🦖 초반 몬스터 진입 애니메이션
            foreach (var frame in enemyFrames)
            {
                Console.Clear();
                Console.WriteLine("\n\n\n");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(frame);
                Console.ResetColor();
                Console.WriteLine("\n\n⚔️ 전투 중...");
                Thread.Sleep(120);
            }

            // ⚔️ 전투 중: 5회 반복 (공룡 애니메이션 + 행동)
            for (int i = 0; i < 5; i++)
            {
                foreach (var frame in enemyFrames)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("\n\n\n" + frame);
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n⚔️ 전투 중...");
                    Console.ResetColor();

                    Console.WriteLine("\n" + battleActions[rand.Next(battleActions.Length)]);
                    Thread.Sleep(120);
                }

                Thread.Sleep(500); // 각 행동 프레임 간 간격
            }

            // ✅ 전투 종료
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n🎉 전투 종료!");
            Console.ResetColor();
            Thread.Sleep(1000);
        }
        #endregion



    }
}
