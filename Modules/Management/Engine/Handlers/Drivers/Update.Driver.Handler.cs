﻿using System.Collections.Generic;
using Trackwane.Framework.Common;
using Trackwane.Framework.Common.Exceptions;
using Trackwane.Framework.Infrastructure.Requests;
using Trackwane.Framework.Interfaces;
using log4net;
using Trackwane.Management.Domain;
using Trackwane.Management.Engine.Commands.Drivers;
using Trackwane.Management.Engine.Services;

namespace Trackwane.Management.Engine.Handlers.Drivers
{
    public class UpdateDriverHandler : TransactionalHandler<UpdateDriver>
    {
        /* Public */

        public UpdateDriverHandler(
            IProvideTransactions transaction,
            IExecutionEngine publisher, ILog log) : 
            base(transaction, log)
        {
        }

        protected override IEnumerable<DomainEvent> Handle(UpdateDriver cmd, IRepository repository)
        {
            var driver = repository.Find<Driver>(cmd.DriverId, cmd.ApplicationKey);

            var organization = repository.Find<Organization>(cmd.OrganizationId, cmd.ApplicationKey);

            if (!string.IsNullOrEmpty(cmd.Name))
            {
                if (organization == null)
                {
                    throw new BusinessRuleException(PhraseBook.Generate(Message.UNKNOWN_ORGANIZATION, cmd.OrganizationId));
                }

                if (organization.Drivers.Contains(cmd.Name))
                {
                    throw new BusinessRuleException(PhraseBook.Generate(Message.DUPLICATE_DRIVER_NAME, cmd.Name));
                }
                
                driver.UpdateName(cmd.Name);
            }

            return driver.GetUncommittedChanges();
        }
    }
}
