using Xunit;
using System;

namespace TemplateParser2022.Test
{
    public class TemplateEngineTests
    {
        private TemplateEngine CreateEngine() => new TemplateEngine();

        /// <summary>
        /// Test engine substitution of a local datasource string property.
        /// </summary>
        [Fact]
        public void Test_basic_local_property_substitute()
        {
            TemplateEngine engine = this.CreateEngine();
            var dataSource = new
            {
                Name = "John"
            };
            
            string output = engine.Apply("Hello [Name]", dataSource);
            Assert.Equal("Hello John", output);
        }

        /// <summary>
        /// Test engine substitution of spanned ([object.property]) datasource string properties.
        /// </summary>
        [Fact]
        public void Test_spanned_local_property_substitute()
        {
            TemplateEngine engine = this.CreateEngine();
            var dataSource = new
            {
                Contact = new
                {
                    FirstName = "John",
                    LastName = "Smith"
                }
            };
            string output = engine.Apply("Hello [Contact.FirstName] [Contact.LastName]", dataSource);
            Assert.Equal("Hello John Smith", output);
        }

        /// <summary>
        /// Test engine substitution of scoped ([with]) datasource string properties.
        /// </summary>
        [Fact]
        public void Test_scoped_property_substitute()
        {
            TemplateEngine engine = this.CreateEngine();
            var dataSource = new
            {
                Contact = new
                {
                    FirstName = "John",
                    LastName = "Smith",
                    Organisation = new
                    {
                        Name = "Acme Ltd",
                        City = "Auckland"
                    }
                }
            };
            string output = engine.Apply(@"[with Contact]Hello [FirstName] from [with Organisation][Name] in [City][/with][/with]", dataSource);
            Assert.Equal("Hello John from Acme Ltd in Auckland", output);
        }

        /// <summary>
        /// Test engine substitution of invalid datasource string properties.
        /// </summary>
        [Fact]
        public void Test_invalid_property_substitute()
        {
            TemplateEngine engine = this.CreateEngine();
            var dataSource = new
            {
                Name = "John"
            };
            string output = engine.Apply(@"Hello [InvalidProperty1] [InvalidProperty2] [Name]", dataSource);
            Assert.Equal("Hello   John", output);
        }

        /// <summary>
        /// Test engine substitution of a template without any tokens.
        /// </summary>
        [Fact]
        public void Test_no_property_substitute()
        {
            TemplateEngine engine = this.CreateEngine();
            var dataSource = new
            {
                Name = "John"
            };
            string output = engine.Apply("Hello there", dataSource);
            Assert.Equal("Hello there", output);
        }

        /// <summary>
        /// OPTIONAL BONUS Test engine substitution of a template with formatted tokens.
        /// </summary>
        [Fact]
        public void Test_formatted_date_property_substitute_bonus()
        {
            TemplateEngine engine = this.CreateEngine();
            var dataSource = new {
                Today = new DateTimeOffset(1990, 12, 1, 0, 0, 0, 0, TimeSpan.Zero)
            };
            string output = engine.Apply("The current date is [Today \"d MMMM yyyy\"]", dataSource);
            Assert.Equal("The current date is 1 December 1990", output);
        }

        /// <summary>
        /// OPTIONAL BONUS Test engine substitution of a template with tokens containing format arguments that should be ignored.
        /// </summary>
        [Fact]
        public void Test_unformattable_property_substitute_bonus()
        {
            TemplateEngine engine = this.CreateEngine();
            var dataSource = new {
                Name = "John"
            };
            string output = engine.Apply("Hello [Name \"d MMMM yyyy\"]", dataSource);
            Assert.Equal("Hello John", output);
        }

        /* EXTENDED TEST SCENARIOS
        [Fact]
        public void Test_scoped_deeply_property_substitute()
        {
            TemplateEngine engine = this.CreateEngine();
            var dataSource = new
            {
                Contact = new
                {
                    FirstName = "John",
                    LastName = "Smith",
                    Organisation = new
                    {
                        Name = "Acme Ltd",
                        City = "Auckland",
                        Address = new
						{
                            Street = "123 Main Street"
						}
                    }
                }
            };
            string output = engine.Apply(@"[with Contact]Hello [FirstName] from [with Organisation][Name] in [City] at [with Address][Street][/with][/with][/with]", dataSource);
            Assert.Equal("Hello John from Acme Ltd in Auckland at 123 Main Street", output);
        }

        [Fact]
        public void Test_scoped_and_spanned_property_substitute()
        {
            TemplateEngine engine = this.CreateEngine();
            var dataSource = new
            {
                Contact = new
                {
                    FirstName = "John",
                    LastName = "Smith",
                    Organisation = new
                    {
                        Name = "Acme Ltd",
                        City = "Auckland"
                    }
                }
            };
            string output = engine.Apply(@"[with Contact]Hello [FirstName] from [Organisation.Name] in [Organisation.City][/with]", dataSource);
            Assert.Equal("Hello John from Acme Ltd in Auckland", output);
        }

        [Fact]
        public void Test_super_spanned_property_substitute()
        {
            TemplateEngine engine = this.CreateEngine();
            var dataSource = new
            {
                Contact = new
                {
                    FirstName = "John",
                    LastName = "Smith",
                    Organisation = new
                    {
                        Name = "Acme Ltd",
                        City = "Auckland"
                    }
                }
            };
            string output = engine.Apply(@"Hello [Contact.FirstName] from [Contact.Organisation.Name] in [Contact.Organisation.City]", dataSource);
            Assert.Equal("Hello John from Acme Ltd in Auckland", output);
        }

        [Fact]
        public void Test_super_spanned_property_substitute_with_format()
        {
            TemplateEngine engine = this.CreateEngine();
            var dataSource = new
            {
                Contact = new
                {
                    FirstName = "John",
                    LastName = "Smith",
                    Organisation = new
                    {
                        Name = "Acme Ltd",
                        City = "Auckland",
                        When = new DateTimeOffset(1990, 12, 1, 0, 0, 0, 0, TimeSpan.Zero)
                    }
                }
            };
            string output = engine.Apply("Hello [Contact.FirstName] from [Contact.Organisation.Name] in [Contact.Organisation.City] at [Contact.Organisation.When \"d MMMM yyyy\"]", dataSource);
            Assert.Equal("Hello John from Acme Ltd in Auckland at 1 December 1990", output);
        }       
        */
    }
}
