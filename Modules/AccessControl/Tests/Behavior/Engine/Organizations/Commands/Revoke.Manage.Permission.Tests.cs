﻿using NUnit.Framework;
using Shouldly;
using Trackwane.AccessControl.Contracts.Events;
using Trackwane.Framework.Common.Exceptions;
using Trackwane.Framework.Fixtures;

namespace Trackwane.AccessControl.Tests.Behavior.Engine.Organizations.Commands
{
    internal class Revoke_Manage_Permission_Tests : Scenario
    {
        private string USER_KEY;
        private string ORGANIZATION_KEY;

        [SetUp]
        public void SetUp()
        {
            USER_KEY = GenerateKey();
            ORGANIZATION_KEY = GenerateKey();

            Register_Organization.With(Persona.SystemManager(), ORGANIZATION_KEY);
            Register_User.With(Persona.SystemManager(), ORGANIZATION_KEY, USER_KEY);
            Grant_Manage_Permission.With(Persona.SystemManager(), ORGANIZATION_KEY, USER_KEY);
        }

        [Test]
        public void When_Successful_Persists_Change()
        {
            Revoke_Manage_Permission.With(Persona.SystemManager(), ORGANIZATION_KEY, USER_KEY);

            var organization = Client.Use(Persona.SystemManager()).Organizations.FindByKey(ORGANIZATION_KEY);
            organization.Managers.ShouldBeEmpty();
        }

        [Test]
        public void When_Successful_Publishes_Event()
        {
            Revoke_Manage_Permission.With(Persona.SystemManager(), ORGANIZATION_KEY, USER_KEY);

            WasPosted<ManagePermissionRevoked>().ShouldBeTrue();
        }

        [Test]
        public void Cannot_Be_Executed_By_Viewers()
        {
            Assert.Throws<UnauthorizedException>(() =>
            {
                Revoke_Manage_Permission.With(Persona.Viewer(ApplicationKey), ORGANIZATION_KEY, USER_KEY);
            });
        }

        [Test]
        public void Cannot_Be_Executed_By_Managers()
        {
            Assert.Throws<UnauthorizedException>(() =>
            {
                Revoke_Manage_Permission.With(Persona.Manager(ApplicationKey), ORGANIZATION_KEY, USER_KEY);
            });
        }

        [Test]
        public void Can_Be_Executed_By_System_Managers()
        {
            Assert.DoesNotThrow(() =>
            {
                Revoke_Manage_Permission.With(Persona.SystemManager(), ORGANIZATION_KEY, USER_KEY);
            });
        }

        [Test]
        public void Can_Be_Executed_By_Administrators()
        {
            Assert.DoesNotThrow(() =>
            {
                Revoke_Manage_Permission.With(Persona.Administrator(ApplicationKey, ORGANIZATION_KEY), ORGANIZATION_KEY, USER_KEY);
            });
        }
    }
}
