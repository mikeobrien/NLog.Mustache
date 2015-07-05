using NLog.Mustache;
using NUnit.Framework;
using Should;

namespace Tests
{
    [TestFixture]
    public class TemplateTests
    {
        [Test]
        public void should_find_template()
        {
            Template.Load("template.html", false)
                .ShouldEqual("<b>Oh hai! {{Level}}</b>");
        }

        [Test]
        public void should_return_empty_debug_message()
        {
            Template.Load("EmptyTemplate.html", true)
                .ShouldEqual("Embedded resource Tests.Templates.EmptyTemplate.html exists but is empty.");
        }

        [Test]
        public void should_return_missing_template_debug_message()
        {
            var template = Template.Load("missing.html", true);
            template.ShouldContain("Embedded resource missing.html not found.");
            template.ShouldContain("Tests, ");
            template.ShouldContain("Tests.Templates.Template.html");
            template.ShouldContain("Tests.Templates.EmptyTemplate.html");
        }
    }
}
