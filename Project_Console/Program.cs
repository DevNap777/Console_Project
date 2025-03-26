using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Numerics;

namespace Project_Console
{
    internal class Program
    {
        struct Position()
        {
            // 게임 내에서 플레이어의 위치 설정
            public int x;
            public int y;
        }
        
        static void Main(string[] args)
        {
            // 게임 오버가 아니면 게임이 계속 돌아가게 만들기
            bool gameOver = false;
            // 구조체에 만든 플레이어 위치 할당
            Position player;
            // char 2차원 배열로 맵 할당
            char[,] map; // 2차원 배열로 첫 번째 맵으로 지정 가능

            Show();

            Start(out player, out map);

                while (gameOver == false)
                {
                    // 그리기
                    Render(player, map);
                    // 출력
                    ConsoleKey key = Input();
                    // 처리
                    Update(key, ref player, map, ref gameOver);
                }
                 End();
        }
        #region 시작 멘트
        static void Show()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆");
            Console.ResetColor();
            Console.WriteLine(" Let's play SOKOBAN");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Press any key to Start");
            Console.ResetColor();

            Console.ReadKey(true);
            Console.Clear();
        }
        #endregion

        #region 게임 시작시 출력되게 해야하는 것들
        static void Start(out Position player, out char[,] map) // 현재 플레이어의 위치를 설정해서 Main 함수 Start로 out처리
        {
            // 콘솔 깜빡임 삭제
            Console.CursorVisible = false;

            // 현재 플레이어 위치 x, y 5
            player.x = 5;
            player.y = 5;
            
            // 맵 만들기
            map = new char[10, 15]
                {
                    // ▨ == 벽
                    // □ == 골
                    // ■ == 박스
                    // ● == 플레이어
                    {'▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨'},
                    {'▨', '□', '▨', ' ', ' ', ' ', ' ', ' ', ' ', '□', ' ', ' ', ' ', ' ', '▨'},
                    {'▨', ' ', '▨', ' ', ' ', ' ', ' ', '■', '▨', '▨', ' ', ' ', ' ', '■', '▨'},
                    {'▨', ' ', ' ', ' ', '■', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '▨'},
                    {'▨', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '■', ' ', ' ', '□', '▨'},
                    {'▨', '▨', '▨', '▨', ' ', ' ', '■', ' ', '▨', '▨', ' ', ' ', ' ', ' ', '▨'},
                    {'▨', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '▨', '▨', ' ', '□', ' ', ' ', '▨'},
                    {'▨', ' ', '□', ' ', ' ', ' ', ' ', ' ', '▨', '▨', ' ', ' ', ' ', ' ', '▨'},
                    {'▨', ' ', ' ', '▨', '▨', '▨', '▨', '▨', '▨', '▨', ' ', ' ', ' ', ' ', '▨'},
                    {'▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨'},
                };
            //map = new char[20, 30]
            //    {
            //        {'▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨'},
            //        {'▨', '□', '▨', ' ', ' ', ' ', ' ', ' ', ' ', '□', ' ', ' ', ' ', ' ', '▨', '▨', '□', '▨', ' ', ' ', ' ', ' ', ' ', ' ', '□', ' ', ' ', ' ', ' ', '▨'},
            //        {'▨', ' ', '▨', ' ', ' ', ' ', ' ', '■', '▨', '▨', ' ', ' ', ' ', '■', '▨', '▨', ' ', '▨', ' ', ' ', ' ', ' ', '■', '▨', '▨', ' ', ' ', ' ', '■', '▨'},
            //        {'▨', ' ', ' ', ' ', '■', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '▨', '▨', ' ', ' ', ' ', '■', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '▨'},
            //        {'▨', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '■', ' ', ' ', '□', '▨', '▨', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '■', ' ', ' ', '□', '▨'},
            //        {'▨', '▨', '▨', '▨', ' ', ' ', '■', ' ', '▨', '▨', ' ', ' ', ' ', ' ', '▨', '▨', '▨', '▨', '▨', ' ', ' ', '■', ' ', '▨', '▨', ' ', ' ', ' ', ' ', '▨'},
            //        {'▨', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '▨', '▨', ' ', '□', ' ', ' ', '▨', '▨', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '▨', '▨', ' ', '□', ' ', ' ', '▨'},
            //        {'▨', ' ', '□', ' ', ' ', ' ', ' ', ' ', '▨', '▨', ' ', ' ', ' ', ' ', '▨', '▨', ' ', '□', ' ', ' ', ' ', ' ', ' ', '▨', '▨', ' ', ' ', ' ', ' ', '▨'},
            //        {'▨', ' ', ' ', '▨', '▨', '▨', '▨', '▨', '▨', '▨', ' ', ' ', ' ', ' ', '▨', '▨', ' ', ' ', '▨', '▨', '▨', '▨', '▨', '▨', '▨', ' ', ' ', ' ', ' ', '▨'},
            //        {'▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨'},
            //        {'▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨'},
            //        {'▨', '□', '▨', ' ', ' ', ' ', ' ', ' ', ' ', '□', ' ', ' ', ' ', ' ', '▨', '▨', '□', '▨', ' ', ' ', ' ', ' ', ' ', ' ', '□', ' ', ' ', ' ', ' ', '▨'},
            //        {'▨', ' ', '▨', ' ', ' ', ' ', ' ', '■', '▨', '▨', ' ', ' ', ' ', '■', '▨', '▨', ' ', '▨', ' ', ' ', ' ', ' ', '■', '▨', '▨', ' ', ' ', ' ', '■', '▨'},
            //        {'▨', ' ', ' ', ' ', '■', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '▨', '▨', ' ', ' ', ' ', '■', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '▨'},
            //        {'▨', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '■', ' ', ' ', '□', '▨', '▨', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '■', ' ', ' ', '□', '▨'},
            //        {'▨', '▨', '▨', '▨', ' ', ' ', '■', ' ', '▨', '▨', ' ', ' ', ' ', ' ', '▨', '▨', '▨', '▨', '▨', ' ', ' ', '■', ' ', '▨', '▨', ' ', ' ', ' ', ' ', '▨'},
            //        {'▨', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '▨', '▨', ' ', '□', ' ', ' ', '▨', '▨', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '▨', '▨', ' ', '□', ' ', ' ', '▨'},
            //        {'▨', ' ', '□', ' ', ' ', ' ', ' ', ' ', '▨', '▨', ' ', ' ', ' ', ' ', '▨', '▨', ' ', '□', ' ', ' ', ' ', ' ', ' ', '▨', '▨', ' ', ' ', ' ', ' ', '▨'},
            //        {'▨', ' ', ' ', '▨', '▨', '▨', '▨', '▨', '▨', '▨', ' ', ' ', ' ', ' ', '▨', '▨', ' ', ' ', '▨', '▨', '▨', '▨', '▨', '▨', '▨', ' ', ' ', ' ', ' ', '▨'},
            //        {'▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨', '▨'},
            //    };
        }
        #endregion

        #region 그려내야 할 것들(플레이어, 맵)
        // 게임에 필요한 것들 어떻게 나타낼지 그리기
        static void Render(Position player, char[,] map)
        {
            Console.SetCursorPosition(0, 0);
            MapPrint(map);
            PlayerPrint(player);
        }
        // 맵 그리기 함수
        static void MapPrint(char[,] map)
        {
            //// 다음 레벨 추가 중
            //int level = 0;

            // 벽 색 변경
            Console.ForegroundColor = ConsoleColor.White;

            // y(행) 범위 만큼 설정
            for (int y = 0; y < map.GetLength(0); y++)
            {
                // x(열) 범위 만큼 설정
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    Console.Write(map[y, x]);
                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }
        // 플레이어 그리기 함수
        static void PlayerPrint(Position player)
        {
            // 플레이어의 위치 덧그리기
            Console.SetCursorPosition(player.x, player.y);
            // 플레이어의 색상 변경
            Console.ForegroundColor = ConsoleColor.Green;
            // 플레이어 형태
            Console.Write('●');
            // 플레이어 색상 초기화
            Console.ResetColor();
        }
        #endregion

        #region 출력되어야 하는 것(이동 키 출력)
        // 출력되는 것들
        static ConsoleKey Input()
        {
            return Console.ReadKey(true).Key;
        }
        #endregion

        #region 처리되어야 할 것들(이동 키 처리)
        // 처리 함수
        static void Update(ConsoleKey key, ref Position player, char[,] map, ref bool gameOver)
        {
            PlayerMove(key, ref player, map);

            bool isClear = IsClear(map);
            if (isClear)
            {
                gameOver = true;
            }
        }
        #endregion

        #region 플레이어 이동 처리 함수
        static void PlayerMove(ConsoleKey key, ref Position player, char[,] map)
        {
            // 박스의 위치 이동과 그 너머의 이동을 위한 변수 선언
            // Position을 쓴 이유는 player의 이동 위치에 따라 달라지기 때문.
            Position frontPosition;
            Position overPosition;

            switch (key)
            {
                // 플레이어가 왼쪽으로 이동할 때
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    // 플레이어가 박스를 만나면 플레이어는 왼쪽으로 한 칸 움직임
                    frontPosition.x = player.x - 1;
                    frontPosition.y = player.y;
                    overPosition.x = player.x - 2;
                    overPosition.y = player.y;
                    break;
                // 플레이어가 오른쪽으로 이동할 때
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    frontPosition.x = player.x + 1;
                    frontPosition.y = player.y;
                    overPosition.x = player.x + 2;
                    overPosition.y = player.y;
                    break;
                // 플레이어가 위쪽으로 이동할 때
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    frontPosition.x = player.x;
                    frontPosition.y = player.y - 1;
                    overPosition.x = player.x;
                    overPosition.y = player.y - 2;
                    break;
                // 플레이어가 아래쪽으로 이동할 때
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    frontPosition.x = player.x;
                    frontPosition.y = player.y + 1;
                    overPosition.x = player.x;
                    overPosition.y = player.y + 2;
                    break;
                default:
                    return;
            }

            // 상황 설정(박스, 골, 벽)
            // 1. 이동 방향에 박스가 있을때
            if (map[frontPosition.y, frontPosition.x] == '■')
            {
                // 1-1. 다음 칸에 골이 있을때
                if (map[overPosition.y, overPosition.x] == '□')
                {
                    // 1-2. 박스를 골 위치로 이동 == 골 박스
                    map[overPosition.y, overPosition.x] = '▣';

                    // 1-3. 박스 위치를 빈 칸으로
                    map[frontPosition.y, frontPosition.x] = ' ';
                }
                // 1-5. 다음 칸에 빈칸이 있을때
                else if (map[overPosition.y, overPosition.x] == ' ')
                {
                    // 1-6. 박스 위치를 빈칸으로
                    map[frontPosition.y, frontPosition.x] = ' ';

                    // 1-7. 빈칸을 박스로
                    map[overPosition.y, overPosition.x] = '■';
                }
                // 1-8. 다음 칸에 벽이 있다면?
                else if (map[overPosition.y, overPosition.x] == '▨')
                {
                    return; // 안 움직임
                }
                // 1-9. 다음 칸에 박스가 있다면?
                else if (map[overPosition.y, overPosition.x] == '■')
                {
                    // 안 움직임
                    return;
                }
            }
            // 2. 이동 방향에 골 박스가 있을때
            else if (map[frontPosition.y, frontPosition.x] == '▣')
            {
                // 2-1.다음 칸에 골이 있을때
                if (map[overPosition.y, overPosition.x] == '□')
                {
                    // 2-2. 박스를 골 위치로 이동 == 골 박스
                    map[overPosition.y, overPosition.x] = '▣';

                    // 2-3. 박스 위치를 골로
                    map[frontPosition.y, frontPosition.x] = '□';
                }
                // 2-5. 다음 칸에 빈칸이 있을때
                else if (map[overPosition.y, overPosition.x] == ' ')
                {
                    // 2-6. 골 박스를 박스으로
                    map[frontPosition.y, frontPosition.x] = '□';
                    // 2-7. 빈칸을 박스로
                    map[overPosition.y, overPosition.x] = '■';
                }
            }
            // 3. 이동 방향에 골이 있거나 빈 칸일때
            else if (map[frontPosition.y, frontPosition.x] == '□' && map[frontPosition.y, frontPosition.x] == ' ')
            {
                player.y = frontPosition.x;
                player.x = frontPosition.y;
            }
            // 4. 이동 방향이 벽일때
            else if (map[frontPosition.y, frontPosition.x] == '▨')
            {
                return; // 안 움직임
            }
            player.x = frontPosition.x;
            player.y = frontPosition.y;
        }
        #endregion

        #region 클리어 조건 함수
        static bool IsClear(char[,]map)
        {
            // 골 숫자가 0이 되면 클리어 조건
            int goalCount = 0;
            foreach (char tile in map)
            {
                if (tile == '□')
                {
                    goalCount++;
                    break;
                }
            }

            if (goalCount == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 게임 종료시 출력되어야 할 것들
        // 게임 종료
        static void End()
        {
            Console.Clear();
            Console.WriteLine("클리어!!!");
        }
        #endregion
    }
}
