﻿using System;
using System.Collections.Generic;
using log4net;
using Trackwane.Framework.Common;
using Trackwane.Framework.Infrastructure.Requests;
using Trackwane.Framework.Interfaces;
using Trackwane.Management.Contracts.Events;
using IProvideTransactions = Trackwane.Framework.Interfaces.IProvideTransactions;
using IRepository = Trackwane.Framework.Interfaces.IRepository;

namespace Trackwane.Data.Engine.Listeners
{
    public class TrackerArchivedListener : TransactionalListener<SensorArchived>
    {
        public TrackerArchivedListener(IProvideTransactions transaction, IExecutionEngine publisher, ILog log) : base(transaction, publisher, log)
        {
        }

        protected override IEnumerable<DomainEvent> Handle(SensorArchived cmd, IRepository repository)
        {
            throw new NotImplementedException();
        }
    }
}
