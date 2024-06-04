
CREATE TABLE [Machinery] (
	[id] UNIQUEIDENTIFIER NOT NULL UNIQUE,
	[Name] TEXT,
	[Origin] TEXT,
	[Model] TEXT,
	[Description] TEXT,
	[Quantity] INT,
	[Status] NVARCHAR(255) CHECK([Status] in ()),
	[SerialNumber] TEXT,
	[StockPrice] FLOAT,
	[SellingPrice] FLOAT,
	[Priority] INT,
	[ImageUrl] TEXT,
	[CategoryId] UNIQUEIDENTIFIER,
	PRIMARY KEY([id])
);
GO

CREATE TABLE [Category] (
	[id] UNIQUEIDENTIFIER NOT NULL UNIQUE,
	[Name] TEXT,
	[Status] NVARCHAR(255) CHECK([Status] in ()),
	[Type] NVARCHAR(255) CHECK([Type] in ()),
	[Description] TEXT,
	[MasterCategory] UNIQUEIDENTIFIER,
	PRIMARY KEY([id])
);
GO

CREATE INDEX [index_0]
ON [Category] ();
GO

CREATE TABLE [Specifications] (
	[id] UNIQUEIDENTIFIER NOT NULL UNIQUE,
	[Name] TEXT,
	[Value] FLOAT,
	[MachineryId] UNIQUEIDENTIFIER,
	PRIMARY KEY([id])
);
GO

CREATE TABLE [MachinePartMachines] (
	[Quantity] INT UNIQUE,
	[Status] NVARCHAR(255) CHECK([Status] in ()),
	[Price ] FLOAT,
	[MachineComponentId] UNIQUEIDENTIFIER,
	[MachineryId] UNIQUEIDENTIFIER,
	PRIMARY KEY([MachineComponentId], [MachineryId])
);
GO

CREATE TABLE [MachineComponents] (
	[id] UNIQUEIDENTIFIER NOT NULL UNIQUE,
	[Name] TEXT,
	[Description] TEXT,
	PRIMARY KEY([id])
);
GO

CREATE TABLE [Order] (
	[id] UNIQUEIDENTIFIER NOT NULL UNIQUE,
	[CreateDate] DATETIME,
	[CompletedDate] DATETIME,
	[Note] TEXT,
	[Status] NVARCHAR(255) CHECK([Status] in ()),
	[FinalAmount] FLOAT,
	[Total] FLOAT,
	[AccountId] UNIQUEIDENTIFIER,
	[PaymentId] UNIQUEIDENTIFIER,
	PRIMARY KEY([id])
);
GO

CREATE TABLE [OrderDetail] (
	[id] UNIQUEIDENTIFIER NOT NULL UNIQUE,
	[Quantity] INT,
	[SellingPrice] FLOAT,
	[OrderId] UNIQUEIDENTIFIER,
	[MachineryId] UNIQUEIDENTIFIER,
	PRIMARY KEY([id])
);
GO

CREATE TABLE [Payment] (
	[id] UNIQUEIDENTIFIER NOT NULL UNIQUE,
	[PaymentMethod] TEXT,
	PRIMARY KEY([id])
);
GO

CREATE TABLE [Account] (
	[id] UNIQUEIDENTIFIER NOT NULL UNIQUE,
	[Username] TEXT,
	[Password] TEXT,
	[Role] NVARCHAR(255) CHECK([Role] in ()),
	[FullName] TEXT,
	[PhoneNumber] TEXT,
	[Address] TEXT,
	[Status] NVARCHAR(255) CHECK([Status] in ()),
	[Email] TEXT,
	[Rank] UNIQUEIDENTIFIER,
	[Amount] FLOAT,
	[YearsOfExperience] INT,
	PRIMARY KEY([id])
);
GO

CREATE TABLE [Certifications] (
	[id] UNIQUEIDENTIFIER NOT NULL UNIQUE,
	[CertificationLink] TEXT,
	[DateObtained] DATETIME,
	[AccountId] UNIQUEIDENTIFIER,
	PRIMARY KEY([id])
);
GO

CREATE TABLE [Maintenance] (
	[id] UNIQUEIDENTIFIER NOT NULL UNIQUE,
	[Type] NVARCHAR(255) CHECK([Type] in ()),
	[CreateDate] DATETIME,
	[StartDate] DATETIME,
	[CompletionDate] DATETIME,
	[Status] NVARCHAR(255) CHECK([Status] in ()),
	[Description] TEXT,
	[Comments] TEXT,
	[NextMaintenanceDate] DATETIME,
	[Priority] INT,
	[TechnicalId] UNIQUEIDENTIFIER,
	[ComponentParts] UNIQUEIDENTIFIER,
	[OrderDetailId] UNIQUEIDENTIFIER,
	PRIMARY KEY([id])
);
GO

CREATE TABLE [MaintenanceDetail] (
	[id] UNIQUEIDENTIFIER NOT NULL UNIQUE,
	[Type] NVARCHAR(255) CHECK([Type] in ()),
	[CreateDate] DATETIME,
	[StartDate] DATETIME,
	[CompletionDate] DATETIME,
	[Status] NVARCHAR(255) CHECK([Status] in ()),
	[Description] TEXT,
	[Comments] TEXT,
	[NextMaintenanceDate] DATETIME,
	[TechnicalId] UNIQUEIDENTIFIER,
	[ComponentParts] UNIQUEIDENTIFIER,
	[OrderDetailId] UNIQUEIDENTIFIER,
	PRIMARY KEY([id])
);
GO

ALTER TABLE [Category]
ADD FOREIGN KEY([id]) REFERENCES [Category]([MasterCategory])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO
ALTER TABLE [Machinery]
ADD FOREIGN KEY([CategoryId]) REFERENCES [Category]([id])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO
ALTER TABLE [Specifications]
ADD FOREIGN KEY([MachineryId]) REFERENCES [Machinery]([id])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO
ALTER TABLE [MachinePartMachines]
ADD FOREIGN KEY([MachineryId]) REFERENCES [Machinery]([id])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO
ALTER TABLE [MachinePartMachines]
ADD FOREIGN KEY([MachineComponentId]) REFERENCES [MachineComponents]([id])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO
ALTER TABLE [Order]
ADD FOREIGN KEY([PaymentId]) REFERENCES [Payment]([id])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO
ALTER TABLE [OrderDetail]
ADD FOREIGN KEY([OrderId]) REFERENCES [Order]([id])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO
ALTER TABLE [OrderDetail]
ADD FOREIGN KEY([MachineryId]) REFERENCES [Machinery]([id])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO
ALTER TABLE [Order]
ADD FOREIGN KEY([AccountId]) REFERENCES [Account]([id])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO
ALTER TABLE [Certifications]
ADD FOREIGN KEY([AccountId]) REFERENCES [Account]([id])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO