using AlexaSkillsService.Data.DontBlowUp;
using System.Data.Entity.Migrations;
using System.Linq;

namespace AlexaSkillsService.ORM.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AlexaSkillsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(AlexaSkillsContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var russia1962 = context.Narratives.SingleOrDefault(n => n.Key == "Russia1962");
            if (russia1962 == null)
            {
                russia1962 = new Narrative
                {
                    Key = "Russia1962",
                    Text = "October twenty-second, nineteen sixty-two. Ten o'clock p.m.: Memo to Special Agent Sydney Ferrel: Agent Ferrel, As " +
                           "you know, ealier tonight, President Kennedy addressed the nation, informing them of the discovery of missile " +
                           "facilities in Cuba, 90 miles from the Florida coast. United States forces have been placed on DEFCON 3. The USS " +
                           "Newport News is en route to stop the Soviet vessel Labinsk from entering Cuban waters. What the president did not " +
                           "announce is that Navy divers have discovered an explosive device attached to the starboard hull of the ship. Your " +
                           "mission: Disembark the USS Leary, rendezvous with the divers, and disarm the device."
                };
                context.Narratives.Add(russia1962);
            }

            var germany1942 = context.Narratives.SingleOrDefault(n => n.Key == "Germany1942");
            if (germany1942 == null)
            {
                germany1942 = new Narrative
                {
                    Key = "Germany1942",
                    Text = "April fifteenth, nineteen forty-two. Three o'clock p.m.: Memo to Special Agent Leon Walker: Agent Walker, in recent " +
                           "months the Germanys set back Allied efforts by creating a new version of their Enigma encryption machine code named Triton. " +
                           "British efforts to decrypt messages encoded with Triton have been unsuccessful. This morning, the USS Greer discovered one " +
                           "of the new machines after taking control of a Germany U-boat in the Carribean, but the device is armed with explosives. Your " +
                           "mission: Locate the the USS Greer, board the captive submarine, and neutralize the Triton. Upon your success, deliver the "+
                           "device to the U.S. Navy's cryptanalysis office immediately."
                };
            }
            context.Narratives.Add(germany1942);

            context.SaveChanges();
        }
    }
}
