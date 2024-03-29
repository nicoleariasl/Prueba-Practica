USE [PruebaP]
GO
/****** Object:  Table [dbo].[ArtVenta]    Script Date: 15/1/2024 16:39:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArtVenta](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [varchar](50) NULL,
	[Nombre] [varchar](50) NULL,
	[Precio] [decimal](18, 0) NULL,
	[IVA] [bit] NULL,
	[Total] [decimal](18, 0) NULL,
 CONSTRAINT [PK_ArtVenta] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Detalle]    Script Date: 15/1/2024 16:39:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Detalle](
	[Id] [int] IDENTITY(100,1) NOT NULL,
	[IdFactura] [int] NULL,
	[CodArticulo] [int] NULL,
	[Cantidad] [int] NULL,
	[Total] [numeric](18, 0) NULL,
 CONSTRAINT [PK_Detalle] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Encabezado]    Script Date: 15/1/2024 16:39:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Encabezado](
	[FacturaId] [int] IDENTITY(100,1) NOT NULL,
	[Fecha] [datetime] NULL,
	[Cliente] [int] NULL,
 CONSTRAINT [PK_Encabezado] PRIMARY KEY CLUSTERED 
(
	[FacturaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 15/1/2024 16:39:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Usuario] [varchar](50) NULL,
	[Clave] [varchar](50) NULL,
	[Nombre] [varchar](150) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ArtVenta] ON 

INSERT [dbo].[ArtVenta] ([Id], [Codigo], [Nombre], [Precio], [IVA], [Total]) VALUES (1, N'ART01', N'Teclado', CAST(1100 AS Decimal(18, 0)), 1, CAST(1243 AS Decimal(18, 0)))
INSERT [dbo].[ArtVenta] ([Id], [Codigo], [Nombre], [Precio], [IVA], [Total]) VALUES (2, N'ART02', N'Mouse', CAST(500 AS Decimal(18, 0)), 0, CAST(500 AS Decimal(18, 0)))
SET IDENTITY_INSERT [dbo].[ArtVenta] OFF
GO
SET IDENTITY_INSERT [dbo].[Usuarios] ON 

INSERT [dbo].[Usuarios] ([Id], [Usuario], [Clave], [Nombre]) VALUES (1, N'test@test.com', N'test123', N'test')
INSERT [dbo].[Usuarios] ([Id], [Usuario], [Clave], [Nombre]) VALUES (2, N'test1@test.com', N'test12', N'test1')
SET IDENTITY_INSERT [dbo].[Usuarios] OFF
GO
ALTER TABLE [dbo].[Detalle]  WITH CHECK ADD  CONSTRAINT [FK_Detalle_Articulo] FOREIGN KEY([CodArticulo])
REFERENCES [dbo].[ArtVenta] ([Id])
GO
ALTER TABLE [dbo].[Detalle] CHECK CONSTRAINT [FK_Detalle_Articulo]
GO
ALTER TABLE [dbo].[Detalle]  WITH CHECK ADD  CONSTRAINT [FK_Detalle_Factura] FOREIGN KEY([IdFactura])
REFERENCES [dbo].[Encabezado] ([FacturaId])
GO
ALTER TABLE [dbo].[Detalle] CHECK CONSTRAINT [FK_Detalle_Factura]
GO
ALTER TABLE [dbo].[Encabezado]  WITH CHECK ADD  CONSTRAINT [FK_Cliente_Factura] FOREIGN KEY([Cliente])
REFERENCES [dbo].[Usuarios] ([Id])
GO
ALTER TABLE [dbo].[Encabezado] CHECK CONSTRAINT [FK_Cliente_Factura]
GO
