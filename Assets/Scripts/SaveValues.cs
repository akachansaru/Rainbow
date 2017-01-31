using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class SaveValues {
    /// <summary>
    /// The maximum number of scores that will be saved plus one to hold the new score 
    /// before it is compared to the other scores. 
    /// Once this number is reached, new scores will replace old scores of lesser value.
    /// </summary>
    public static int maxScoreCapacity = 51;

    public List<int> highScores;
}
