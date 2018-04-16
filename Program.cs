using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardsAlgorithm
{
       
    public class Cards
    {
        public sbyte Type;//类型 包括 红桃1 方块2 梅花3 黑桃4 
        public sbyte Number;//1-13 A~K 13表示K 1表示A 
        
        public sbyte CType;//CalculateType 计算类型 
        public sbyte Weight;//权值 用来计算的
        public Cards()
        {
        }
        public Cards(sbyte type, sbyte weight)
        {
            Type = type;
            Weight = weight;
        }
     
    }
    /// <summary>
    /// 游戏类型
    /// </summary>
    public enum GameType
    {
        DOUDIZHU
    }

    public enum GameCardPattern
    {
        NONE,
        SINGLE,
        DOUBLE,
        TRIPLE,
        QUADRA,
        ROKECT,
        TRIPLEANDONE,
        TRIPANDTWO,
        QUADRAANDTWO,
        QUADRAADNFOUR,
        SINGLESTRAIGHT,
        DOUBLESTRAIGHT,
        PLANE,
        PLANEANDONEWING,
        PLANDANDTWOWING
    }

    public class CardPattern
    {
        //同
        public sbyte same;
        //顺
        public sbyte straight;

        public List<sbyte> value=new List<sbyte>();
        public CardPattern(sbyte same,sbyte straight)
        {
            this.same = same;
            this.straight = straight;
        }
    }
    public class CardsPatterns
    {
        //public Dictionary<Cards, CardPattern> cardsDict = new Dictionary<Cards, CardPattern>();
        public List<CardPattern> cardsPatternsList = new List<CardPattern>();
    }

    public class CardsMethod
    {

        /// <summary>
        /// 一副牌的创建
        /// </summary>
        /// <param name="gameType"    表示游戏类型></param>
        /// <returns></returns>
        public virtual List<Cards> CreateCards(GameType gameType)
        {
            List<Cards> cardList = new List<Cards>();

            //添加一个牌头 用来表示这副牌的类型
            Cards head = new Cards
            {
                Type = 0,
                Weight = 0
            };
            cardList.Add(head);


            //添加从A~K的四种花色的牌 K表示13 2表示15 A表示14
            for ( sbyte i = 3; i <= 15; i++)
            {
                for (sbyte j = 1; j <= 4; j++)
                {
                    Cards card = new Cards
                    {
                        Type = j,
                        Weight = i
                    };
                    if (card.Weight <= 13)
                    {
                        card.Number = card.Weight;
                    }
                    else
                    {
                        card.Number =(sbyte)(card.Weight % 13);
                    }
                    cardList.Add(card);
                }      
            }
            //根据游戏类型  确定大小王的添加
            if (gameType == GameType.DOUDIZHU)
            {
                Cards smallKing = new Cards
                {
                    Type = 5,
                    Weight = 99,
                    Number = 99
                };
                cardList.Add(smallKing);

                Cards bigKing = new Cards
                {
                    Type = 5,
                    Weight = 100,                 
                    Number = 100                 
                };
                cardList.Add(bigKing);
            }
            return cardList;
        }
        /// <summary>
        /// 交换两张牌
        /// </summary>
        /// <param name="card1"></param>
        /// <param name="card2"></param>
        public void Swap(ref Cards card1,ref Cards card2)
        {
            Cards temp = card1;
            card1 = card2;
            card2 = temp;
        }
        /// <summary>
        /// 洗牌
        /// </summary>
        /// <param name="cardList" 传入要洗的牌 ></param>
        public virtual List<Cards> Shuffle(List<Cards> cardList )
        {
            Random rd = new Random();
            int index;
            for (int i = 1; i < cardList.Count-1; i++)
            {
                
                index= rd.Next(1, cardList.Count);
                
                Cards tempCard = cardList[i];
                cardList[i] = cardList[index];
                cardList[index]=tempCard;

            }
            //foreach (var temp in cardList)
            //{
            //    Console.Write(temp.Number + " ");
            //}
            return cardList;
        }
        /// <summary>
        /// 分牌操作 并且返回需要留下的牌
        /// </summary>
        /// <param name="gameType" 游戏类型></param>
        /// <param name="cardList" 传入的牌 ></param>
        /// <param name="cardList1"玩家一的牌 ></param>
        /// <param name="cardList2"玩家二的牌 ></param>
        /// <param name="cardList3"玩家三的牌 ></param>
        /// <returns></returns>
        public virtual List<Cards> HandleCards(GameType gameType,List<Cards> cardList,out List<Cards> cardList1,out List<Cards> cardList2,out List<Cards> cardList3)
        {
            cardList1 = new List<Cards>();
            cardList2 = new List<Cards>();
            cardList3 = new List<Cards>();

            List<Cards> leftCardsList = new List<Cards>();

            //需要留下多少张牌
            int leftCards = 0;
            int leftIndex=0;

            //判读是哪种游戏类型
            switch (gameType)
            {
                case GameType.DOUDIZHU:
                    leftCards = 3;
                    break;
            }
            //分牌
            for ( int i = 1; i < cardList.Count-leftCards; i += 3)
            {
                cardList1.Add(cardList[i]);
                cardList2.Add(cardList[i + 1]);
                cardList3.Add(cardList[i + 2]);

                leftIndex = i;
            }
            //返回留下的牌 从留下的位置的下一个开始
            for (int i = leftIndex+1; i < cardList.Count; i++)
            {
                leftCardsList.Add(cardList[i]);
            }

            return leftCardsList;
        }

        public virtual void SendCards(GameType type,List<Cards> cardList)
        {
            List<Cards> cardList1=new List<Cards>();
            List<Cards> cardList2=new List<Cards>();
            List<Cards> cardList3 = new List<Cards>() ;

            List<Cards> leftList= HandleCards(type, cardList, out cardList1, out cardList2, out cardList3);

            foreach (var temp in leftList)
            {
                cardList3.Add(temp);
            }
            /*
             此处处理发完牌后的操作 例如

             foreach(var temp in leftList)
             {
                cardList3.Add(temp);
             }

            */

            SortCards(cardList1,true);
            SortCards(cardList2,true);
            SortCards(cardList3,true);
            

            //Console.Write("一号玩家的牌：");
            //foreach (var temp in cardList1)
            //{
            //    Console.Write(temp.Number+" ");
            //}
            //Console.WriteLine();

            //Console.Write("二号玩家的牌：");
            //foreach (var temp in cardList2)
            //{
            //    Console.Write(temp.Number + " ");
            //}
            //Console.WriteLine();

            //Console.Write("三号玩家的牌：");
            //foreach (var temp in cardList3)
            //{
            //    Console.Write(temp.Number + " ");
            //}
            //Console.WriteLine();
        }

        /// <summary>
        /// 给传入的牌 排序
        /// </summary>
        /// <param name="cardList" 需要传入的牌></param>
        /// <param name="sequence" true表示正序 false 表示倒序></param>
        public virtual void SortCards(List<Cards> cardList,bool sequence)
        {

            //1 表示正序 -1表示倒序
            int dir = 1;
            if (sequence)
            {
                dir = 1;
            }
            else
            {
                dir = -1;
            }
            cardList.Sort((Comparison<Cards>)((Cards card1, Cards card2) =>
            {
                
                return (int)(dir* card1.Weight.CompareTo((sbyte)card2.Weight));
            }));
        }
        public virtual void SortByTimes(List<Cards> list)
        {
            
        }
        /// <summary>
        /// 得到牌的数字大小的字符串
        /// </summary>
        /// <param name="cardList"></param>
        /// <returns></returns>
        public string GetCardStrs(List<Cards> cardList)
        {
            StringBuilder sb = new StringBuilder();

            SortCards(cardList, true);
            foreach (var card in cardList)
            {
                sb.Append(card.Weight);
            }

            return sb.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardList"要传入判断的牌></param>
        /// <param name="length"  要传入判断的牌的长度></param>
        /// <returns></returns>
        public virtual List<CardPattern> JudgeCardsPattern(List<Cards> cardList,int length)
        {
            //表示储存牌型的字典
            //Dictionary<CardPattern,List<string>> cardsPatternDict = new Dictionary<CardPattern,List<string>>();
            //表示牌串的数组
            //List<string> stringList = new List<string>();
            //表示同样牌的张数
            Dictionary<Cards, int> sameDict = new Dictionary<Cards, int>();
            //表示相同的牌的数组
            List<Cards> sameList = new List<Cards>();
            //表示一串牌的几种牌型
            List<CardPattern> cardPatternList = new List<CardPattern>();
            //先给牌进行排序
            SortCards(cardList, true);

            //第一步 先寻找相同的牌的  并且用存储牌和对应的个数 例如一张3 两张5 存储代表3-1 5-2
            int sameCount = 1;
            for (int i = 0; i < length- 1; i++)
            {
                if (cardList[i].Weight == cardList[i + 1].Weight)
                {
                    sameCount++;
                }
                else
                {
                    //如果有相同的牌的键 则不存入字典
                    if (!sameDict.ContainsKey(cardList[i]))
                    {
                        sameDict.Add(cardList[i], sameCount);
                        sameList.Add(cardList[i]);
                    }
                    sameCount = 1;
                }
            }
            //添加最后一个牌
            if (!sameDict.ContainsKey(cardList[length-1])&&cardList.Count>1)
            {
                sameDict.Add(cardList[length - 1], sameCount);
                sameList.Add(cardList[length - 1]);
            }


            foreach (var t in sameDict)
            {
                Console.WriteLine(t.Key.Weight+"-"+t.Value);
            }

            //建立四个数组 分别用来存储 拥有1 2 3 4 张牌的牌是哪些牌
            List<Cards> list1 = new List<Cards>();
            List<Cards> list2 = new List<Cards>();
            List<Cards> list3 = new List<Cards>();
            List<Cards> list4 = new List<Cards>();

            //foreach (var tem in sameList)
            //{
            //    Console.WriteLine(tem.Number);
            //}
            //所有的牌都有1张 所以 数组相同
            list1 = sameList;
            for (int i = 0; i < sameList.Count; i++)
            {
                switch (sameDict[sameList[i]])
                {
                    case 1:
                        //list1.Add(sameList[i]);
                        break;
                    case 2:
                        list2.Add(sameList[i]);
                        break;
                    case 3:
                        list2.Add(sameList[i]);
                        list3.Add(sameList[i]);
                        break;
                    case 4:
                        //如过一种牌 它有四张 那么 在有两张 三张的牌里面 也应该有它
                        list2.Add(sameList[i]);
                        list3.Add(sameList[i]);
                        list4.Add(sameList[i]);
                        break;
                }
            }


            //判断牌的种类

            if (cardList!=null && cardList.Count > 0)
            {
                JudgeSimpleCardPattern(list1, 1, 1);
                JudgeSimpleCardPattern(list2, 2, 1);
                JudgeCompexCardPattern(list1, 1, 5);
                JudgeCompexCardPattern(list2, 2, 3);
                JudgeCompexCardPattern(list3, 3, 1);
                JudgeCompexCardPattern(list4, 4, 1);
            }
            

            void JudgeSimpleCardPattern(List<Cards> list,sbyte samecount,sbyte straightcount)
            {
                if (list != null && list.Count > 0)
                {
                    foreach (var card in list)
                    {
                        CardPattern cardPattern = new CardPattern(samecount, straightcount);
                        cardPattern.value = new List<sbyte> { card.Weight };
                        cardPatternList.Add(cardPattern);
                    }
                }

            }
            //list表示传入的种类的牌的数组 samecount表示这种牌的张数 straightCount表示要达到顺子应该至少的个数
            //判断复杂牌型  主要包含顺子 三张 四张 
            void JudgeCompexCardPattern(List<Cards> list,sbyte samecount,sbyte straightCount)
            {
                if (list != null && list.Count > 2)
                {

                    sbyte Count = 1;//表示连续次数
                    int pre = 0;//表示之前判断到的点
                    int next = 0;//表示现在判断到的点
                    for (int i = 0; i < list.Count-1; i++)
                    {
                        if (list[i].Weight+1== list[i + 1].Weight)
                        {
                            Count++;//如果符合条件 连续次数+1
                        }
                        else
                        {
                            //否则 记录下判断到的点
                            next = i;
                           
                            if (Count >= straightCount)
                            {

                                //属于哪一种 牌型 例如 3同1顺
                                CardPattern cardPattern = new CardPattern(samecount, Count);

                                for (int j = pre; j <= next; j++)
                                {
                                    //判断到的点之前的牌全部需要存储下来
                                    cardPattern.value.Add(list[j].Weight);
                                }
                                cardPatternList.Add(cardPattern);
                                //判断到的牌添加进字典
                            }
                            //重置连续的次数
                            Count = 1;
                            //把这次判断到的点  赋给上次判断到的点
                            pre = next+1;
                        }
                    }
                    //表示判断到最后一个点的时候 此时判断到List[Count-1]      
                    if (list[list.Count - 2].Weight + 1 == list[list.Count - 1].Weight)
                    {
                        if (Count >= straightCount)
                        {
                            CardPattern cardPattern = new CardPattern(samecount, Count);
                            for (int j = pre; j <= list.Count - 1; j++)
                            {
                                //判断到的点之前的牌全部需要存储下来
                                cardPattern.value.Add(list[j].Weight);
                            }
                            cardPatternList.Add(cardPattern);
                        }

                    }
                    else if (list[list.Count - 2].Weight + 1 != list[list.Count - 1].Weight)
                    {
                        //此时判断最后一点  如果是此类型牌含有三张以上才加入复杂牌型
                        if (samecount > 2)
                        {
                            CardPattern cardPattern = new CardPattern(samecount, Count);
                            cardPattern.value.Add(list[list.Count - 1].Weight);
                            cardPatternList.Add(cardPattern);
                        }

                    }
                            
                 
                }
            }

            foreach (var temp in cardPatternList)
            {
                foreach (var t in temp.value)
                {
                    Console.Write(t);
                }
                Console.WriteLine(" "+temp.same + "同" + temp.straight + "顺");
            }
            

            return cardPatternList;
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            CardsMethod cm = new CardsMethod();
            List<Cards> cardList= cm.CreateCards(GameType.DOUDIZHU);
            cm.Shuffle(cardList);
            cm.SendCards(GameType.DOUDIZHU, cardList);

            List<Cards> list1 = new List<Cards>();


            //list1.Add(new Cards(1, 2));
            //list1.Add(new Cards(1, 3));
            //list1.Add(new Cards(1, 3));
            //list1.Add(new Cards(1, 3));
            //list1.Add(new Cards(1, 3));
            //list1.Add(new Cards(1, 4));
            //list1.Add(new Cards(1, 4));
            //list1.Add(new Cards(1, 4));
            //list1.Add(new Cards(1, 5));
            //list1.Add(new Cards(1, 5));
            //list1.Add(new Cards(1, 5));
            //list1.Add(new Cards(1, 5));
            //list1.Add(new Cards(1, 6));
            //list1.Add(new Cards(1, 7));
            //list1.Add(new Cards(1, 8));
            //list1.Add(new Cards(1, 9));
            //list1.Add(new Cards(1, 10));
            //list1.Add(new Cards(1, 10));
            //list1.Add(new Cards(1, 10));
            //list1.Add(new Cards(1, 11));
            //list1.Add(new Cards(1, 12));
            //list1.Add(new Cards(1, 13));


            cm.JudgeCardsPattern(list1,list1.Count);


        }
    }
}
