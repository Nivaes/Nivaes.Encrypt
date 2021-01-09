
namespace Nivaes.Encrypt.UnitTest
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Xunit;
    using FluentAssertions;
    using System.Threading.Tasks;

    [Trait("TestType", "Unit")]
    public class EncryptTest
    {
        [Fact]
        public async Task AsyncLazySuccess1()
        {
            var originMessage = "kjkdkdif";
            var encriptedMessage = await EncryptHelper.Encrypt(originMessage, "pass").ConfigureAwait(true);

            encriptedMessage.Should().NotBeNull();

            var decryptMessage = await EncryptHelper.Decrypt(encriptedMessage, "pass").ConfigureAwait(true);

            decryptMessage.Should().Be(originMessage);
        }

        [Theory]
        [InlineData("Hola mundo", "pass")]
        [InlineData("alsdhfahsdfj akdfjas dfjalñks kl", "dkdf38834$$·33")]
        [InlineData("", "")]
        [InlineData("", "kdk33.55%")]
        [InlineData("jaklsdfjkdjasñf aaksjdf ñlakjfñ kajñf kañlfj aklsfj aksdfjaslñie", "1")]
        [InlineData("kajsdñfklas dfaksjf ñadfj ñdfjsie", "138382929.30293+ç")]
        public async Task AsyncLazySuccess2(string message, string pass)
        {
            var encriptedMessage = await EncryptHelper.Encrypt(message, pass).ConfigureAwait(true);

            encriptedMessage.Should().NotBeNull();

            var decryptMessage = await EncryptHelper.Decrypt(encriptedMessage, pass).ConfigureAwait(true);

            decryptMessage.Should().Be(message);
        }
    }
}
