using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Unity.Services.RemoteConfig.Authoring.Editor.Formatting;

namespace Unity.Services.RemoteConfig.Tests.Editor.Authoring.Formatting
{
    public class ConfigTypeDeriverTests
    {
        ConfigTypeDeriver m_ConfigTypeDeriver;

        [SetUp]
        public void SetUp()
        {
            m_ConfigTypeDeriver = new ConfigTypeDeriver();
        }

        [Test]
        public void IntProvided_IntDerived()
        {
            var value = CreateJValue(10);
            var derivedType = m_ConfigTypeDeriver.DeriveType(value);
            
            Assert.AreEqual(derivedType, ConfigType.INT);
        }
        
        [Test]
        public void StringProvided_StringDerived()
        {
            var value = CreateJValue("string");
            var derivedType = m_ConfigTypeDeriver.DeriveType(value);
            
            Assert.AreEqual(derivedType, ConfigType.STRING);
        }
        
        [Test]
        public void BoolProvided_BoolDerived()
        {
            var value = CreateJValue(false);
            var derivedType = m_ConfigTypeDeriver.DeriveType(value);
            
            Assert.AreEqual(derivedType, ConfigType.BOOL);
        }
        
        [Test]
        public void LongProvided_LongDerived()
        {
            var value = CreateJValue((long) 10);
            var derivedType = m_ConfigTypeDeriver.DeriveType(value);
            
            Assert.AreEqual(derivedType, ConfigType.LONG);
        }
        
        [Test]
        public void FloatProvided_FloatDerived()
        {
            var value = CreateJValue(1f);
            var derivedType = m_ConfigTypeDeriver.DeriveType(value);
            
            Assert.AreEqual(derivedType, ConfigType.FLOAT);
        }

        [Test]
        public void JsonProvided_JsonDerived()
        {
            var value = new JObject();
            var derivedType = m_ConfigTypeDeriver.DeriveType(value);
            
            Assert.AreEqual(derivedType, ConfigType.JSON);
        }
        
        [Test]
        public void DoubleProvided_FloatDerived()
        {
            var configTypeDeriver = new ConfigTypeDeriver();
            var token = new JValue(1.2);
            var value = configTypeDeriver.DeriveType(token);
            Assert.AreEqual(ConfigType.FLOAT, value);
        }

        JValue CreateJValue(object value)
        {
            return new JValue(value);
        }
    }
}
