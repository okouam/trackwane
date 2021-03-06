﻿using Trackwane.Framework.Infrastructure.Requests;

namespace Trackwane.Management.Engine.Commands.Drivers
{
    public class UpdateDriver : UserCommand
    {
        public string DriverId { get; private set; }

        public string Name { get; set; }

        public UpdateDriver(string applicationKey, string requesterId, string organizationId, string driverId) : base(applicationKey, requesterId)
        {
            DriverId = driverId;
            OrganizationId = organizationId;
        }

        public string OrganizationId { get; private set; }
    }
}
