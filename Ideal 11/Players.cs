using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ideal_11
{
    public class Players
    {
       //player data
        public string id {  get; set; } //int
        public string rank {  get; set; } //int
        public string name  {  get; set; }
        public string gender { get; set; } //char
        public string height { get; set; } //int
        public string weight { get; set; } //int
        public string age { get; set; } //int
        public string position { get; set; }
        [JsonPropertyName("weak foot")]
        public string WeakFoot { get; set; } //int
        [JsonPropertyName("skill moves")]
        public string SkillMoves { get; set; } //int
        [JsonPropertyName("preferred foot")]
        public string PreferredFoot { get; set; }
        [JsonPropertyName("alternative positions")]
        public string AlternativePositions { get; set; }

        //location
        public string nation { get; set; }
        public string league { get; set; }
        public string team { get; set; }
        [JsonPropertyName("play style")]
        public List<string> PlayStyle { get; set; }

        //ratings
        public string ovr { get; set; }
        public string pac { get; set; }
        public string sho { get; set; }
        public string pas { get; set; }
        public string dri { get; set; }
        public string def { get; set; }
        public string phy { get; set; }
        public string acceleration { get; set; }
        [JsonPropertyName("sprint speed")]
        public string SprintSpeed { get; set; }
        public string positioning { get; set; }
        public string finishing { get; set; }
        [JsonPropertyName("shot power")]
        public string ShotPower { get; set; }
        [JsonPropertyName("long shots")]
        public string LongShots { get; set; }
        public string volleys { get; set; }
        public string penalties { get; set; }
        public string vision { get; set; }
        public string crossing { get; set; }
        [JsonPropertyName("free kick accuracy")]
        public string FreeKickAccuracy { get; set; }
        [JsonPropertyName("short passing")]
        public string short_passing { get; set; }
        [JsonPropertyName("long passing")]
        public string LongPassing { get; set; }

        public string curve { get; set; }
        public string dribbling { get; set; }
        public string agility { get; set; }
        public string balance { get; set; }
        public string reactions { get; set; }
        [JsonPropertyName("ball control")]
        public string BallControl { get; set; }
        public string composure { get; set; }
        public string interceptions { get; set; }
        [JsonPropertyName("heading accuracy")]
        public string HeadingAccuracy { get; set; }
        [JsonPropertyName("def awareness")]
        public string DefAwareness { get; set; }
        [JsonPropertyName("standing tackle")]
        public string StandingTackle { get; set; }
        [JsonPropertyName("sliding tackle")]
        public string SlidingTackle { get; set; }
        public string jumping { get; set; }
        public string stamina { get; set; }
        public string strength { get; set; }
        public string aggression { get; set; }

        //gk ratings
        public string url { get; set; }
        public string diving { get; set; } //int
        public string handling { get; set; } //int
        public string kicking { get; set; } //int
        public string reflexes { get; set; } //int
        public string card { get; set; }
        public string game { get; set; }

    }
}
