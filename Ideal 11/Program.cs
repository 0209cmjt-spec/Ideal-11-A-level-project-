using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;


namespace Ideal_11
{ 
    internal class Program
    {
        static List<Players> all_comparisons = new List<Players> { };
        static List<Players> oplayer(string id)
        {
            WebRequest request = WebRequest.Create("https://api.msmc.cc/api/eafc/players?game=fc26&update=2&id=" + id);
            request.Method = WebRequestMethods.Http.Get;
            request.ContentType = "application/json; charset=utf8;";
            string text;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }
            List <Players> opponentplayer = JsonSerializer.Deserialize<List<Players>>(text);
            return opponentplayer;
        }
        static List<Players> comparisonplayer(string ovr,string position)
        {
            WebRequest request = WebRequest.Create("https://api.msmc.cc/api/eafc/players?game=fc26&update=2&ovr>" + ovr + "&position=" + position);
            request.Method = WebRequestMethods.Http.Get;
            request.ContentType = "application/json; charset=utf8;";
            string text;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }
            List<Players> comparplayer = JsonSerializer.Deserialize<List<Players>>(text);
            return comparplayer;
        }
        



        static Players Comparison(string id,string names,string type,List<Players> playerinfo)
        {
            
            //opponents player
            List<Players> oppoplayer = oplayer(id);
            string ovrall = oppoplayer[0].ovr;
            string position = oppoplayer[0].position;   

            //best so far
            double highestscore = 0;
            string gplayer = "";

            //all current comparisons
            List<Players> cplayer = new List<Players> { };
            List<string> playerIDs = new List<string> { };
            List<Players> callplayer = new List<Players> { };
            string comp_overall = "";
            if (names == "no" && type == "N")
            {
                comp_overall = (int.Parse(ovrall) - 10).ToString();
                cplayer = comparisonplayer(comp_overall, position);
                callplayer = cplayer;
                all_comparisons = cplayer;
                playerIDs = new List<string>();
                foreach (Players p in cplayer)
                {
                    if (playerIDs.Contains(p.id) == false)
                    {
                        playerIDs.Add(p.id);
                    }
                }
            }
            else if (type == "C")
            {
                callplayer = playerinfo;
                all_comparisons = playerinfo;
                for (int i = 0; i < callplayer.Count; i++)
                {
                    if (callplayer[i].nation == oppoplayer[0].nation)
                    {
                        cplayer.Add(callplayer[i]);
                    }
                }
                playerIDs = new List<string>();
                foreach (Players p in cplayer)
                {
                    if (playerIDs.Contains(p.id) == false)
                    {
                        playerIDs.Add(p.id);
                    }
                }
            }
            else
            {
                callplayer = playerinfo;
                all_comparisons = playerinfo;
                for (int i = 0; i < callplayer.Count; i++)
                {
                    if (callplayer[i].team == oppoplayer[0].team)
                    {
                        cplayer.Add(callplayer[i]);
                    }
                }
                playerIDs = new List<string>();
                foreach (Players p in cplayer)
                {
                    if (playerIDs.Contains(p.id) == false)
                    {
                        playerIDs.Add(p.id);
                    }
                }
            }

            int besti = 0;

            //comparsion 
            for (int i = 0; i < playerIDs.Count; i++)
            {
               besti = 0;
               double innerhs = 0;
               if (oppoplayer[0].position != "GK")
               {
                    //If attacking then shooting counts as 3 if not only 1
                    if (oppoplayer[0].position != "CDM" || oppoplayer[0].position != "CB")
                    {
                        if (int.Parse(cplayer[i].sho) <= int.Parse(oppoplayer[0].sho))
                        {
                            innerhs += 2;
                        }
                        if (int.Parse(cplayer[i].finishing) <= int.Parse(oppoplayer[0].finishing))
                        {
                            innerhs++;
                        }
                        if (int.Parse(cplayer[i].ShotPower) <= int.Parse(oppoplayer[0].ShotPower))
                        {
                            innerhs++;
                        }
                        if (int.Parse(cplayer[i].volleys) <= int.Parse(oppoplayer[0].volleys))
                        {
                            innerhs++;
                        }
                        if (int.Parse(cplayer[i].LongShots) <= int.Parse(oppoplayer[0].LongShots))
                        {
                            innerhs += 0.5;
                        }
                        if (int.Parse(cplayer[i].WeakFoot) <= int.Parse(oppoplayer[0].WeakFoot))
                        {
                            innerhs++;
                        }

                    }
                    if (int.Parse(cplayer[i].sho) <= int.Parse(oppoplayer[0].sho))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].finishing) <= int.Parse(oppoplayer[0].finishing))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].ShotPower) <= int.Parse(oppoplayer[0].ShotPower))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].LongShots) <= int.Parse(oppoplayer[0].LongShots))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].volleys) <= int.Parse(oppoplayer[0].volleys))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].WeakFoot) <= int.Parse(oppoplayer[0].WeakFoot))
                    {
                        innerhs++;
                    }

                    //If defensive then defending counts as 3 if not only 1
                    if (oppoplayer[0].position == "CDM" || oppoplayer[0].position == "CB")
                    {
                        if (int.Parse(cplayer[i].def) <= int.Parse(oppoplayer[0].def))
                        {
                            innerhs += 2;
                        }
                        if (int.Parse(cplayer[i].DefAwareness) <= int.Parse(oppoplayer[0].DefAwareness))
                        {
                            innerhs++;
                        }
                        if (int.Parse(cplayer[i].SlidingTackle) <= int.Parse(oppoplayer[0].SlidingTackle))
                        {
                            innerhs++;
                        }
                        if (int.Parse(cplayer[i].StandingTackle) <= int.Parse(oppoplayer[0].StandingTackle))
                        {
                            innerhs++;
                        }
                    }
                    if (int.Parse(cplayer[i].def) <= int.Parse(oppoplayer[0].def))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].DefAwareness) <= int.Parse(oppoplayer[0].DefAwareness))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].SlidingTackle) <= int.Parse(oppoplayer[0].SlidingTackle))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].StandingTackle) <= int.Parse(oppoplayer[0].StandingTackle))
                    {
                        innerhs++;
                    }



                    if (int.Parse(cplayer[i].pac) <= int.Parse(oppoplayer[0].pac))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].pas) <= int.Parse(oppoplayer[0].pas))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].dri) <= int.Parse(oppoplayer[0].dri))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].phy) <= int.Parse(oppoplayer[0].phy))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].acceleration) <= int.Parse(oppoplayer[0].acceleration))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].SprintSpeed) <= int.Parse(oppoplayer[0].SprintSpeed))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].positioning) <= int.Parse(oppoplayer[0].positioning))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].penalties) <= int.Parse(oppoplayer[0].penalties))
                    {
                        innerhs += 1.5;
                    }
                    if (int.Parse(cplayer[i].vision) <= int.Parse(oppoplayer[0].vision))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].crossing) <= int.Parse(oppoplayer[0].crossing))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].FreeKickAccuracy) <= int.Parse(oppoplayer[0].FreeKickAccuracy))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].short_passing) <= int.Parse(oppoplayer[0].short_passing))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].LongPassing) <= int.Parse(oppoplayer[0].LongPassing))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].curve) <= int.Parse(oppoplayer[0].curve))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].dribbling) <= int.Parse(oppoplayer[0].dribbling))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].agility) <= int.Parse(oppoplayer[0].agility))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].balance) <= int.Parse(oppoplayer[0].balance))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].reactions) <= int.Parse(oppoplayer[0].reactions))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].BallControl) <= int.Parse(oppoplayer[0].BallControl))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].composure) <= int.Parse(oppoplayer[0].composure))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].interceptions) <= int.Parse(oppoplayer[0].interceptions))
                    {
                        innerhs += 1.5;
                    }
                    if (int.Parse(cplayer[i].HeadingAccuracy) <= int.Parse(oppoplayer[0].HeadingAccuracy))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].jumping) <= int.Parse(oppoplayer[0].jumping))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].stamina) <= int.Parse(oppoplayer[0].stamina))
                    {
                        innerhs += 1.5;
                    }
                    if (int.Parse(cplayer[i].strength) <= int.Parse(oppoplayer[0].strength))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].aggression) <= int.Parse(oppoplayer[0].aggression))
                    {
                        innerhs++;
                    }
                    if (int.Parse(cplayer[i].SkillMoves) <= int.Parse(oppoplayer[0].SkillMoves))
                    {
                        innerhs += 1.5;
                    }

               }
               else
               {
                  if (int.Parse(cplayer[i].diving) <= int.Parse(oppoplayer[0].diving))
                  {
                     innerhs++;
                  }
                  if (int.Parse(cplayer[i].handling) <= int.Parse(oppoplayer[0].handling))
                  {
                      innerhs++;
                  }
                  if (int.Parse(cplayer[i].kicking) <= int.Parse(oppoplayer[0].kicking))
                  {
                      innerhs++;
                  }
                  if (int.Parse(cplayer[i].reflexes) <= int.Parse(oppoplayer[0].reflexes))
                  {
                      innerhs++;
                  }
               }

                if (innerhs > highestscore)
                {
                    highestscore = innerhs;
                    gplayer = cplayer[i].id;
                    besti = i;
                }

            }
           
            return cplayer[besti];
        }
        static Players subComparison(List<Players> myTeam,int x)
        {
            

            // all current relevant players
            List<Players> Relevants = new List<Players> { };
            for (int i = 0; i < all_comparisons.Count; i++)
            {
                if (all_comparisons[i].position == myTeam[x].position)
                {
                    Relevants.Add(all_comparisons[i]);
                }
            }
            
            
            int bestplayer = 0;
            double best_t = 0;
            
            for (int i = 0; i < Relevants.Count; i++)
            {
                double t = 0;
                double dotProd = 0;
                double magA = 0;
                double magB = 0;
                dotProd += int.Parse(myTeam[x].pac) * int.Parse(Relevants[i].pac);
                magA += int.Parse(myTeam[x].pac) * int.Parse(myTeam[x].pac);
                magB += int.Parse(Relevants[i].pac) * int.Parse(Relevants[i].pac);
                
                dotProd += int.Parse(myTeam[x].sho) * int.Parse(Relevants[i].sho);
                magA += int.Parse(myTeam[x].sho) * int.Parse(myTeam[x].sho);
                magB += int.Parse(Relevants[i].sho) * int.Parse(Relevants[i].sho);
                
                dotProd += int.Parse(myTeam[x].finishing) * int.Parse(Relevants[i].finishing);
                magA += int.Parse(myTeam[x].finishing) * int.Parse(myTeam[x].finishing);
                magB += int.Parse(Relevants[i].finishing) * int.Parse(Relevants[i].finishing);
                
                dotProd += int.Parse(myTeam[x].ShotPower) * int.Parse(Relevants[i].ShotPower);
                magA += int.Parse(myTeam[x].ShotPower) * int.Parse(myTeam[x].ShotPower);
                magB += int.Parse(Relevants[i].ShotPower) * int.Parse(Relevants[i].ShotPower);
                
                dotProd += int.Parse(myTeam[x].volleys) * int.Parse(Relevants[i].volleys);
                magA += int.Parse(myTeam[x].volleys) * int.Parse(myTeam[x].volleys);
                magB += int.Parse(Relevants[i].volleys) * int.Parse(Relevants[i].volleys);
                
                dotProd += int.Parse(myTeam[x].LongShots) * int.Parse(Relevants[i].LongShots);
                magA += int.Parse(myTeam[x].LongShots) * int.Parse(myTeam[x].LongShots);
                magB += int.Parse(Relevants[i].LongShots) * int.Parse(Relevants[i].LongShots);
                
                dotProd += int.Parse(myTeam[x].SkillMoves) * int.Parse(Relevants[i].SkillMoves);
                magA += int.Parse(myTeam[x].SkillMoves) * int.Parse(myTeam[x].SkillMoves);
                magB += int.Parse(Relevants[i].SkillMoves) * int.Parse(Relevants[i].SkillMoves);
                
                dotProd += int.Parse(myTeam[x].WeakFoot) * int.Parse(Relevants[i].WeakFoot);
                magA += int.Parse(myTeam[x].WeakFoot) * int.Parse(myTeam[x].WeakFoot);
                magB += int.Parse(Relevants[i].WeakFoot) * int.Parse(Relevants[i].WeakFoot);
                
                dotProd += int.Parse(myTeam[x].pas) * int.Parse(Relevants[i].pas);
                magA += int.Parse(myTeam[x].pas) * int.Parse(myTeam[x].pas);
                magB += int.Parse(Relevants[i].pas) * int.Parse(Relevants[i].pas);
                
                dotProd += int.Parse(myTeam[x].dri) * int.Parse(Relevants[i].dri);
                magA += int.Parse(myTeam[x].dri) * int.Parse(myTeam[x].dri);
                magB += int.Parse(Relevants[i].dri) * int.Parse(Relevants[i].dri);
                
                dotProd += int.Parse(myTeam[x].def) * int.Parse(Relevants[i].def);
                magA += int.Parse(myTeam[x].def) * int.Parse(myTeam[x].def);
                magB += int.Parse(Relevants[i].def) * int.Parse(Relevants[i].def);
                
                dotProd += int.Parse(myTeam[x].phy) * int.Parse(Relevants[i].phy);
                magA += int.Parse(myTeam[x].phy) * int.Parse(myTeam[x].phy);
                magB += int.Parse(Relevants[i].phy) * int.Parse(Relevants[i].phy);
                
                dotProd += int.Parse(myTeam[x].acceleration) * int.Parse(Relevants[i].SprintSpeed);
                magA += int.Parse(myTeam[x].acceleration) * int.Parse(myTeam[x].SprintSpeed);
                magB += int.Parse(Relevants[i].acceleration) * int.Parse(Relevants[i].SprintSpeed);
                
                dotProd += int.Parse(myTeam[x].SprintSpeed) * int.Parse(Relevants[i].SprintSpeed);
                magA += int.Parse(myTeam[x].SprintSpeed) * int.Parse(myTeam[x].SprintSpeed);
                magB += int.Parse(Relevants[i].SprintSpeed) * int.Parse(Relevants[i].SprintSpeed);
                
                dotProd += int.Parse(myTeam[x].positioning) * int.Parse(Relevants[i].positioning);
                magA += int.Parse(myTeam[x].positioning) * int.Parse(myTeam[x].positioning);
                magB += int.Parse(Relevants[i].positioning) * int.Parse(Relevants[i].positioning);
                
                dotProd += int.Parse(myTeam[x].penalties) * int.Parse(Relevants[i].penalties);
                magA += int.Parse(myTeam[x].penalties) * int.Parse(myTeam[x].penalties);
                magB += int.Parse(Relevants[i].penalties) * int.Parse(Relevants[i].penalties);
                
                dotProd += int.Parse(myTeam[x].vision) * int.Parse(Relevants[i].vision);
                magA += int.Parse(myTeam[x].vision) * int.Parse(myTeam[x].vision);
                magB += int.Parse(Relevants[i].vision) * int.Parse(Relevants[i].vision);
                
                dotProd += int.Parse(myTeam[x].crossing) * int.Parse(Relevants[i].crossing);
                magA += int.Parse(myTeam[x].crossing) * int.Parse(myTeam[x].crossing);
                magB += int.Parse(Relevants[i].crossing) * int.Parse(Relevants[i].crossing);
                
                dotProd += int.Parse(myTeam[x].FreeKickAccuracy) * int.Parse(Relevants[i].FreeKickAccuracy);
                magA += int.Parse(myTeam[x].FreeKickAccuracy) * int.Parse(myTeam[x].FreeKickAccuracy);
                magB += int.Parse(Relevants[i].FreeKickAccuracy) * int.Parse(Relevants[i].FreeKickAccuracy);
                
                dotProd += int.Parse(myTeam[x].short_passing) * int.Parse(Relevants[i].short_passing);
                magA += int.Parse(myTeam[x].short_passing) * int.Parse(myTeam[x].short_passing);
                magB += int.Parse(Relevants[i].short_passing) * int.Parse(Relevants[i].short_passing);
                
                dotProd += int.Parse(myTeam[x].LongPassing) * int.Parse(Relevants[i].LongPassing);
                magA += int.Parse(myTeam[x].LongPassing) * int.Parse(myTeam[x].LongPassing);
                magB += int.Parse(Relevants[i].LongPassing) * int.Parse(Relevants[i].LongPassing);
                
                dotProd += int.Parse(myTeam[x].curve) * int.Parse(Relevants[i].curve);
                magA += int.Parse(myTeam[x].curve) * int.Parse(myTeam[x].curve);
                magB += int.Parse(Relevants[i].curve) * int.Parse(Relevants[i].curve);
                
                dotProd += int.Parse(myTeam[x].dribbling) * int.Parse(Relevants[i].dribbling);
                magA += int.Parse(myTeam[x].dribbling) * int.Parse(myTeam[x].dribbling);
                magB += int.Parse(Relevants[i].dribbling) * int.Parse(Relevants[i].dribbling);
                
                dotProd += int.Parse(myTeam[x].agility) * int.Parse(Relevants[i].agility);
                magA += int.Parse(myTeam[x].agility) * int.Parse(myTeam[x].agility);
                magB += int.Parse(Relevants[i].agility) * int.Parse(Relevants[i].agility);
                
                dotProd += int.Parse(myTeam[x].balance) * int.Parse(Relevants[i].balance);
                magA += int.Parse(myTeam[x].balance) * int.Parse(myTeam[x].balance);
                magB += int.Parse(Relevants[i].balance) * int.Parse(Relevants[i].balance);
                
                dotProd += int.Parse(myTeam[x].reactions) * int.Parse(Relevants[i].reactions);
                magA += int.Parse(myTeam[x].reactions) * int.Parse(myTeam[x].reactions);
                magB += int.Parse(Relevants[i].reactions) * int.Parse(Relevants[i].reactions);
               
                dotProd += int.Parse(myTeam[x].BallControl) * int.Parse(Relevants[i].BallControl);
                magA += int.Parse(myTeam[x].BallControl) * int.Parse(myTeam[x].BallControl);
                magB += int.Parse(Relevants[i].BallControl) * int.Parse(Relevants[i].BallControl);
               
                dotProd += int.Parse(myTeam[x].composure) * int.Parse(Relevants[i].composure);
                magA += int.Parse(myTeam[x].composure) * int.Parse(myTeam[x].composure);
                magB += int.Parse(Relevants[i].composure) * int.Parse(Relevants[i].composure);
                
                dotProd += int.Parse(myTeam[x].interceptions) * int.Parse(Relevants[i].interceptions);
                magA += int.Parse(myTeam[x].interceptions) * int.Parse(myTeam[x].interceptions);
                magB += int.Parse(Relevants[i].interceptions) * int.Parse(Relevants[i].interceptions);
                
                dotProd += int.Parse(myTeam[x].HeadingAccuracy) * int.Parse(Relevants[i].HeadingAccuracy);
                magA += int.Parse(myTeam[x].HeadingAccuracy) * int.Parse(myTeam[x].HeadingAccuracy);
                magB += int.Parse(Relevants[i].HeadingAccuracy) * int.Parse(Relevants[i].HeadingAccuracy);
                
                dotProd += int.Parse(myTeam[x].DefAwareness) * int.Parse(Relevants[i].DefAwareness);
                magA += int.Parse(myTeam[x].DefAwareness) * int.Parse(myTeam[x].DefAwareness);
                magB += int.Parse(Relevants[i].DefAwareness) * int.Parse(Relevants[i].DefAwareness);
                
                dotProd += int.Parse(myTeam[x].StandingTackle) * int.Parse(Relevants[i].StandingTackle);
                magA += int.Parse(myTeam[x].StandingTackle) * int.Parse(myTeam[x].StandingTackle);
                magB += int.Parse(Relevants[i].StandingTackle) * int.Parse(Relevants[i].StandingTackle);
                
                dotProd += int.Parse(myTeam[x].SlidingTackle) * int.Parse(Relevants[i].SlidingTackle);
                magA += int.Parse(myTeam[x].SlidingTackle) * int.Parse(myTeam[x].SlidingTackle);
                magB += int.Parse(Relevants[i].SlidingTackle) * int.Parse(Relevants[i].SlidingTackle);
                
                dotProd += int.Parse(myTeam[x].jumping) * int.Parse(Relevants[i].jumping);
                magA += int.Parse(myTeam[x].jumping) * int.Parse(myTeam[x].jumping);
                magB += int.Parse(Relevants[i].jumping) * int.Parse(Relevants[i].jumping);
               
                dotProd += int.Parse(myTeam[x].stamina) * int.Parse(Relevants[i].stamina);
                magA += int.Parse(myTeam[x].stamina) * int.Parse(myTeam[x].stamina);
                magB += int.Parse(Relevants[i].stamina) * int.Parse(Relevants[i].stamina);
                
                dotProd += int.Parse(myTeam[x].strength) * int.Parse(Relevants[i].strength);
                magA += int.Parse(myTeam[x].strength) * int.Parse(myTeam[x].strength);
                magB += int.Parse(Relevants[i].strength) * int.Parse(Relevants[i].strength);
                
                dotProd += int.Parse(myTeam[x].aggression) * int.Parse(Relevants[i].aggression);
                magA += int.Parse(myTeam[x].aggression) * int.Parse(myTeam[x].aggression);
                magB += int.Parse(Relevants[i].aggression) * int.Parse(Relevants[i].aggression);






                t = dotProd / Math.Sqrt(magA * magB);
                if(t < best_t)
                {
                    bestplayer = i;
                    best_t = t;
                }
            }
            return Relevants[bestplayer];
        }


        //Gets the whole users team 
        static List<Players> Genteam(List<string> oppoteam)
        {
            Console.Clear();
            Console.WriteLine("Loading...");
            string s = "no";
            List<Players> myTeam = new List<Players> { };
            for (int i = 0; i < 11; i++)
            {
                myTeam.Add(Comparison(oppoteam[i],s,"N",null));
            }
            return myTeam;
        }
        static List<Players> GenteamC(List<string> oppoteam,string country,List<Players> playerinfo)
        {
            Console.Clear();
            Console.WriteLine("Loading...");
            List<Players> myTeam = new List<Players> { };
            for (int i = 0; i < 11; i++)
            {
                myTeam.Add(Comparison(oppoteam[i],country,"C",playerinfo));
            }
            return myTeam;
        }
        static List<Players> GenteamT(List<string> oppoteam,string team,List<Players> playerinfo)
        {
            Console.Clear();
            Console.WriteLine("Loading...");
            List<Players> myTeam = new List<Players> { };
            for (int i = 0; i < 11; i++)
            {
                myTeam.Add(Comparison(oppoteam[i],team,"T",playerinfo));
            }
            return myTeam;
        }
        static List<Players> Subgen(List<Players> playerinfo)
        {
            Console.Clear();
            Console.WriteLine("Loading...");
            List<Players> sTeam = new List<Players>();
            for (int i = 0; i < 7; i++)
            {
                sTeam.Add(subComparison(playerinfo,i));
            }
            return sTeam;
        }

        //gets the indivual player info
        static string Playersid(string name)
        {
            bool incomplete = true;
            while(incomplete)
            {
                WebRequest request = WebRequest.Create("https://api.msmc.cc/api/eafc/players?game=fc26&update=2&name=" + name);
                request.Method = WebRequestMethods.Http.Get;
                request.ContentType = "application/json; charset=utf8;";
                string text;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    text = sr.ReadToEnd();  
                }
                List<Players> playersinfo = JsonSerializer.Deserialize<List<Players>>(text);
                List<string> playerIDs = new List<string>();
                foreach (Players p in playersinfo)
                {
                    if (playerIDs.Contains(p.id) == false)
                    {
                        playerIDs.Add(p.id);
                    }
                }
                if (playerIDs.Count == 0)
                {
                    return null;
                }
                else if (playerIDs.Count >= 1)
                {
                    for (int i = 0; i < playerIDs.Count; i++)
                    {
                        Console.Clear();
                        Console.WriteLine("Player name:" + playersinfo[i].name);
                        Console.WriteLine("Is this the player that you wanted Y/N");
                        string answer = Console.ReadLine().ToUpper();
                        bool playerfound = false;
                        if (answer == "Y")
                        {
                            return playersinfo[0].id;
                        }
                        else if (answer != "N" || answer != "Y") { return null; }
                        else if (!playerfound && i == playerIDs.Count) { return null; }

                    }
                    
                }
                
                
            }
            return null;
               
        }
        static (string namecheck, List<Players> playerInf) CountryandTeamChecker (string name,string type)
        {
            bool incomplete = true;
            while (incomplete)
            {
                if (type == "C")
                {
                    WebRequest request = WebRequest.Create("https://api.msmc.cc/api/eafc/players?game=fc26&update=2&nation=" + name);
                    request.Method = WebRequestMethods.Http.Get;
                    request.ContentType = "application/json; charset=utf8;";
                    string text;
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        text = sr.ReadToEnd();
                    }
                    List<Players> playersinfo = JsonSerializer.Deserialize<List<Players>>(text);
                    List<string> playerIDs = new List<string>();
                    foreach (Players p in playersinfo)
                    {
                        if (playerIDs.Contains(p.id) == false)
                        {
                            playerIDs.Add(p.id);
                        }
                    }
                    if (playerIDs.Count == 0)
                    {
                        Console.WriteLine("Name the Country and be careful for spelling and punctuation");
                        name = Console.ReadLine();
                        CountryandTeamChecker(name, type);
                    }
                    else
                    {
                        return (name,playersinfo);
                    }
                    
                }
                else 
                {
                    WebRequest request = WebRequest.Create("https://api.msmc.cc/api/eafc/players?game=fc26&update=2&team=" + name);
                    request.Method = WebRequestMethods.Http.Get;
                    request.ContentType = "application/json; charset=utf8;";
                    string text;
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        text = sr.ReadToEnd();
                    }
                    List<Players> playersinfo = JsonSerializer.Deserialize<List<Players>>(text);
                    List<string> playerIDs = new List<string>();
                    foreach (Players p in playersinfo)
                    {
                        if (playerIDs.Contains(p.id) == false)
                        {
                            playerIDs.Add(p.id);
                        }
                    }
                    if (playerIDs.Count == 0)
                    {
                        Console.WriteLine("Name the Team and be careful for spelling and punctuation");
                        name = Console.ReadLine();
                        CountryandTeamChecker(name, type);
                    }
                    else if (playerIDs.Count > 1)
                    {
                        for (int i = 0; i < playerIDs.Count; i++)
                        {
                            
                            if (playersinfo[i].team == playersinfo[i--].team)
                            {
                                i++;
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Player name:" + playersinfo[i].team);
                                Console.WriteLine("Is this the player that you wanted Y/N");
                                string answer = Console.ReadLine().ToUpper();
                                bool teamfound = false;
                                if (answer == "Y")
                                {
                                    return (name, playersinfo);
                                }
                                else if (answer != "N" || answer != "Y") { Console.WriteLine("Please enter either Y/N"); i--; }
                                else if (!teamfound && i == playerIDs.Count)
                                {
                                    Console.WriteLine("Name the Team and be careful for spelling and punctuation");
                                    name = Console.ReadLine();
                                    CountryandTeamChecker(name, type);
                                }
                            }
                        }
                    }
                    else
                    {
                        return (name, playersinfo);
                    }
                    
                }
                


            }
            return (null,null);
        }
        
        //gets whole opponent team from user
        static List<string> oppo()
        {
            
            string pname = "";
            List<string> opponents = new List<string> { };
            for (int i = 0; i < 11; i++)
            {
                bool correct = false;
                if (i == 0)
                {
                    while(!correct)
                    {
                        Console.WriteLine("enter the name of a player on their team");
                        pname = Console.ReadLine();
                        var result = Playersid(pname);
                        if (result == null)
                        {
                            Console.WriteLine("Please re-enter the name of your Player and be careful of punctuation and spelling");
                            pname = Console.ReadLine();
                            result = Playersid(pname);
                        }
                        else
                        {
                            opponents.Add(result);
                            correct = true;
                        }
                    }
                }
                else
                {
                    while (!correct)
                    {
                        Console.WriteLine("enter the name of another player on their team");
                        pname = Console.ReadLine();
                        var result = Playersid(pname);
                        if (result == null)
                        {
                            Console.WriteLine("Please re-enter the name of your Player and be careful of punctuation and spelling");
                            pname = Console.ReadLine();
                            result = Playersid(pname);

                        }
                        else
                        {
                            opponents.Add(result);
                            correct = true;
                        }
                    }
                }

            }
            return opponents;
        }
        static bool finished()
        {
            bool finsher1 = false;
            while (!finsher1)
            {
                Console.WriteLine("do you want to stop using the generation software? Y/N");
                string ans4 = Console.ReadLine().ToUpper();
                if (ans4 == "Y")
                {
                    Console.Clear();
                    Console.WriteLine("Good bye");
                    return true;
                }
                else if (ans4 == "N")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Try again and input either Y/N");
                    break;
                }
            }
            return true;
        }

        static void Main(string[] args)
        {

            bool running = true;
            while (running)
            {
                Console.WriteLine("Are you playing any of the above game modes(Kick off,Seasons,Career Mode(not including any transfers made),Tournaments)? Y/N");
                string ans = Console.ReadLine().ToUpper();
                if (ans == "Y")
                {
                    List<string> opponents = oppo();
                    bool answered = false;
                    while (!answered)
                    {
                        Console.WriteLine("Are you playing as a Country or a Team?  *Answer as C/T ");
                        string ans1 = Console.ReadLine().ToUpper();
                        if (ans1 == "C")
                        {
                            Console.WriteLine("Name the Country");
                            string country = Console.ReadLine().ToUpper();
                            var(country1,playerinfo) = CountryandTeamChecker(country,ans1);
                            List<Players> myTeam = GenteamC(opponents,country1,playerinfo);
                            for (int i = 0; i < myTeam.Count; i++)
                            {
                                Console.WriteLine(myTeam[i].name);
                            }
                            answered = true;
                        }
                        else if (ans1 == "T")
                        {
                            Console.WriteLine("Name the Team");
                            string team = Console.ReadLine().ToUpper();
                            var (team1, playerinfo) = CountryandTeamChecker(team,ans1);
                            List<Players> myTeam = GenteamT(opponents,team1,playerinfo);
                            for (int i = 0; i < myTeam.Count; i++)
                            {
                                Console.WriteLine(myTeam[i].name);
                            }
                            answered = true;
                        }
                        else
                        {
                            Console.WriteLine("Please enter either C/T");
                            break ;
                        }
                    }

                    
                }
                else if (ans == "N")
                {
                    Console.WriteLine("Are you playing Ultimate team? Y/N");
                    string ans2 = Console.ReadLine().ToUpper();
                    if (ans2 == "Y")
                    {
                        Console.WriteLine("It will only be able to account for base cards at this moment is that ok? Y/N");
                        string ans3 = Console.ReadLine().ToUpper();
                        if (ans3 == "Y")
                        {
                            List<string> opponents = oppo();
                            List<Players> myTeam = Genteam(opponents);
                            Console.WriteLine("Would you like to generate 7 subs aswell");
                            string ans4 = Console.ReadLine().ToUpper();
                            if(ans4 == "Y")
                            {
                                Subgen(myTeam);
                            }
                            else
                            {
                                break;
                            }
                            for (int i = 0; i < myTeam.Count; i++)
                            {
                                Console.WriteLine(myTeam[i].name + ", ");
                            }
                        }
                        else if (ans3 == "N")
                        {
                            bool done = finished();
                            if (done)
                            {
                                running = false;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    bool done = finished();
                    if (done) 
                    {
                        running = false; 
                    }
                    else
                    {
                        Console.WriteLine("Try again and input either Y/N");
                        break;
                    }
                }
            }
            
            
        


        }
    }
}
