﻿using System.Collections.Generic;
using log4net;
using Trackwane.Framework.Common;
using Trackwane.Framework.Common.Exceptions;
using Trackwane.Framework.Infrastructure.Requests;
using Trackwane.Framework.Interfaces;
using Trackwane.Management.Contracts.Events;
using Trackwane.Management.Domain;
using Message = Trackwane.Management.Engine.Services.Message;

namespace Trackwane.Management.Engine.Listeners.Organizations.Drivers
{
    public class DriverRegisteredListener : TransactionalListener<DriverRegistered>
    {
        public DriverRegisteredListener(IProvideTransactions transaction, IExecutionEngine publisher, ILog log) : base(transaction, publisher, log)
        {
        }

        protected override IEnumerable<DomainEvent> Handle(DriverRegistered evt, IRepository repository)
        {
            var organization = repository.Find<Organization>(evt.OrganizationKey, evt.ApplicationKey);

            if (organization == null)
            {
                throw new BusinessRuleException(PhraseBook.Generate(Message.UNKNOWN_ORGANIZATION, evt.OrganizationKey));
            }

            organization.Drivers.Add(evt.Name);

            return organization.GetUncommittedChanges();
        }
    }
}
