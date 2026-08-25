using AdminApp.Extensions;
using FluentAssertions;
using Xunit;

namespace AdminApp.Tests.Extensions
{
    public class LuceneQueryEscaperTests
    {
        [Theory]
        [InlineData("+", "\\+")]
        [InlineData("-", "\\-")]
        [InlineData("&&", "\\&\\&")]
        [InlineData("||", "\\|\\|")]
        [InlineData("!", "\\!")]
        [InlineData("(", "\\(")]
        [InlineData(")", "\\)")]
        [InlineData("{", "\\{")]
        [InlineData("}", "\\}")]
        [InlineData("[", "\\[")]
        [InlineData("]", "\\]")]
        [InlineData("^", "\\^")]
        [InlineData("\"", "\\\"")]
        [InlineData("~", "\\~")]
        [InlineData("*", "\\*")]
        [InlineData("?", "\\?")]
        [InlineData(":", "\\:")]
        [InlineData("\\", "\\\\")]
        [InlineData("/", "\\/")]
        public void Escape_EscapesEachLuceneSpecialCharacter(string input, string expected)
        {
            LuceneQueryEscaper.Escape(input).Should().Be(expected);
        }

        [Fact]
        public void Escape_LeavesPlainAlphanumericTermUnchanged()
        {
            LuceneQueryEscaper.Escape("Acme Corp 123").Should().Be("Acme Corp 123");
        }

        [Fact]
        public void Escape_NeutralizesInjectedQuerySyntax()
        {
            var malicious = "Acme\" OR _id:*";

            var escaped = LuceneQueryEscaper.Escape(malicious);

            escaped.Should().Be("Acme\\\" OR _id\\:\\*");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Escape_ReturnsInputUnchanged_WhenNullOrEmpty(string? input)
        {
            LuceneQueryEscaper.Escape(input!).Should().Be(input);
        }
    }
}
