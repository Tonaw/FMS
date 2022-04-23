
using System.Collections.Generic;
using FMS.Data.Models;

namespace FMS.Web.Models
{   
    public class MotSearchViewModel
    {
        // TBC - complete view model to contain 

        // results which is a list of Mot
        public IList<Mot> Mots {get; set;} = new List <Mot>();
       
        // query which is a string 
        public string Query {get; set;}  = string.Empty;

        // range - a MOT range that defaults to Fail

        public TestStatus Range { get; set;} = TestStatus.Fail;

    }
}
