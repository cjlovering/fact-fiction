﻿namespace FactOrFictionTextHandling.Luis
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ILuisClient
    {
        string Query(string sentenceFragment);
    }
}
