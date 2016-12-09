using System.Data.Entity.Migrations;
using System.Linq;
using AlexaSkillsService.Data.DontBlowUp;

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

            var languageRussian = context.Languages.SingleOrDefault(l => l.Name == "Russian");
            if (languageRussian == null)
            {
                languageRussian = new Language { Name = "Russian" };
                context.Languages.Add(languageRussian);
                context.SaveChanges();
            }

            var languageChinese = context.Languages.SingleOrDefault(l => l.Name == "Chinese");
            if (languageChinese == null)
            {
                languageChinese = new Language { Name = "Chinese" };
                context.Languages.Add(languageChinese);
                context.SaveChanges();
            }

            var languageGerman = context.Languages.SingleOrDefault(l => l.Name == "German");
            if (languageGerman == null)
            {
                languageGerman = new Language { Name = "German" };
                context.Languages.Add(languageGerman);
                context.SaveChanges();
            }

            var languageJapanese = context.Languages.SingleOrDefault(l => l.Name == "Japanese");
            if (languageJapanese == null)
            {
                languageJapanese = new Language { Name = "Japanese" };
                context.Languages.Add(languageJapanese);
                context.SaveChanges();
            }

            var wireColorRed = context.WireColors.SingleOrDefault(wc => wc.Name == "Red");
            if (wireColorRed == null)
            {
                wireColorRed = new WireColor { Name = "Red" };
                context.WireColors.Add(wireColorRed);
                context.SaveChanges();
            }

            var wireColorBlue = context.WireColors.SingleOrDefault(wc => wc.Name == "Blue");
            if (wireColorBlue == null)
            {
                wireColorBlue = new WireColor { Name = "Blue" };
                context.WireColors.Add(wireColorBlue);
                context.SaveChanges();
            }

            var wireColorYellow = context.WireColors.SingleOrDefault(wc => wc.Name == "Yellow");
            if (wireColorYellow == null)
            {
                wireColorYellow = new WireColor { Name = "Yellow" };
                context.WireColors.Add(wireColorYellow);
                context.SaveChanges();
            }

            var wireColorWhite = context.WireColors.SingleOrDefault(wc => wc.Name == "White");
            if (wireColorWhite == null)
            {
                wireColorWhite = new WireColor { Name = "White" };
                context.WireColors.Add(wireColorWhite);
                context.SaveChanges();
            }

            var wireColorBlack = context.WireColors.SingleOrDefault(wc => wc.Name == "Black");
            if (wireColorBlack == null)
            {
                wireColorBlack = new WireColor { Name = "Black" };
                context.WireColors.Add(wireColorBlack);
                context.SaveChanges();
            }

            //TODO: Seed YearRanges.
        }
    }
}
