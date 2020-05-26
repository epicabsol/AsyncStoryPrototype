using System;
using AsyncStory.Core;
using ExplorationStory.World;

namespace ExplorationStory
{
    public interface IExplorationStoryHost : IStoryHost
    {
        void UpdateMap(Room[,] map);
    }
}
