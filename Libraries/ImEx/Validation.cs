using System;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;

namespace ImEx
{
    public static class Validation
    {
        public static bool ValidatePerson (string p)
        {
            const string SchemaPerson = @"{
                'description':'A person',
                'type':'object',
                'properties':
                {
                    'FirstName':{'type':'string'},
                    'LastName':{'type':'string'},
                    'Age':{'type':'integer'}
                }
            }";
            JsonSchema Schema = JsonSchema.Parse (SchemaPerson);
            JObject person = JObject.Parse (p);

            bool isValid = person.IsValid (Schema);

            return isValid;

        }
    }
}

