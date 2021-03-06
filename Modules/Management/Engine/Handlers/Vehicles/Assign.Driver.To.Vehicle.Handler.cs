﻿using System.Collections.Generic;
using log4net;
using Trackwane.Framework.Common;
using Trackwane.Framework.Common.Exceptions;
using Trackwane.Framework.Infrastructure.Requests;
using Trackwane.Framework.Interfaces;
using Trackwane.Management.Domain;
using Trackwane.Management.Engine.Commands.Vehicles;
using Trackwane.Management.Engine.Services;

namespace Trackwane.Management.Engine.Handlers.Vehicles
{
    public class AssignDriverToVehicleHandler : TransactionalHandler<AssignDriverToVehicle>
    {
        public AssignDriverToVehicleHandler(
            IProvideTransactions transaction,
            IExecutionEngine publisher,
            ILog log) :
            base(transaction, log)
        {
        }

        protected override IEnumerable<DomainEvent> Handle(AssignDriverToVehicle cmd, IRepository repository)
        {
            var vehicle = repository.Find<Vehicle>(cmd.VehicleId, cmd.ApplicationKey);

            if (vehicle == null)
            {
                throw new BusinessRuleException(PhraseBook.Generate(Message.UNKNOWN_VEHICLE, cmd.VehicleId));
            }

            var driver = repository.Find<Driver>(cmd.DriverId, cmd.ApplicationKey);

            if (driver == null)
            {
                throw new BusinessRuleException(PhraseBook.Generate(Message.UNKNOWN_DRIVER, cmd.DriverId));
            }

            vehicle.AssignDriver(driver);

            return vehicle.GetUncommittedChanges();
        }
    }
}
