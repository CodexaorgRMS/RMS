using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Application.Features.ProductBatches.Commands.CheckExpiring;


// Command executed by the background scheduler to evaluate batch expirations
public sealed record CheckExpiringBatchesCommand;