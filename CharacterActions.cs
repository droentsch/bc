using System;
using System.Collections;
using System.Collections.Generic;

namespace bc
{
    public class CharacterActions
    {
        public string CharacterValue {get;set;}
        public CharCategory CharacterCategory { get; set; }
        public Action<Stack<double>> CharacterFunc { get; set; }
    }

}
