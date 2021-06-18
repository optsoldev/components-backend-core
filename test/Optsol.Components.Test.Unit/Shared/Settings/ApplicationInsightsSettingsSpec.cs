using FluentAssertions;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Settings;
using System;
using System.Collections.Generic;
using Xunit;

namespace Optsol.Components.Test.Unit.Shared.Settings
{
    public class ApplicationInsightsSettingsSpec
    {
        public static IEnumerable<object[]> GetSettings()
        {
            yield return new object[] { new ApplicationInsightsSettings(), new ApplicationInsightsSettings() { InstrumentationKey = "InstrumentationKey" } };
        }

        [Trait("Settings", "ApplicationInsights")]
        [Theory(DisplayName = "Deve validar settings")]
        [MemberData(nameof(GetSettings))]
        public void Deve_Validar_Settings(ApplicationInsightsSettings invalid, ApplicationInsightsSettings valid)
        {
            //Given
            //Theory

            //When
            Action action = () => valid.Validate();
            Action actionInvalid = () => invalid.Validate();

            //Then
            action.Should().NotThrow();
            actionInvalid.Should().ThrowExactly<SettingsNullException>();
        }
    }
}
