﻿using System.Collections.Generic;
using Trackwane.Framework.Common;
using Trackwane.Framework.Common.Exceptions;
using Trackwane.Framework.Infrastructure.Requests;
using Trackwane.Framework.Interfaces;
using Trackwane.Management.Domain;
using Trackwane.Management.Engine.Commands.Vehicles;
using Trackwane.Management.Engine.Services;
using log4net;

namespace Trackwane.Management.Engine.Handlers.Vehicles
{
    public class ArchiveVehicleHandler : TransactionalHandler<ArchiveVehicle>
    {
        public ArchiveVehicleHandler(
            IProvideTransactions transaction,
            IExecutionEngine publisher,
            ILog log) :
            base(transaction, log)
        {
        }
        
        protected override IEnumerable<DomainEvent> Handle(ArchiveVehicle cmd, IRepository repository)
        {
            var vehicle = repository.Find<Vehicle>(cmd.VehicleId, cmd.ApplicationKey);

            if (vehicle == null)
            {
                throw new BusinessRuleException(PhraseBook.Generate(Message.UNKNOWN_VEHICLE, cmd.VehicleId));
            }

            vehicle.Archive();

            return vehicle.GetUncommittedChanges();
        }
    }
}
