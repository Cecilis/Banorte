USE [CuentasPagarBD]
GO
/****** Object:  Table [dbo].[EstatusFactura]    Script Date: 14/01/2018 07:32:47 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EstatusFactura](
	[ID] [int] NULL,
	[statu] [varchar](3) NOT NULL,
	[txtst] [varchar](20) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[EstatusFactura] ([ID], [statu], [txtst]) VALUES (1, N'001', N'En Proceso SAT')
INSERT [dbo].[EstatusFactura] ([ID], [statu], [txtst]) VALUES (2, N'002', N'Aprobada')
INSERT [dbo].[EstatusFactura] ([ID], [statu], [txtst]) VALUES (3, N'003', N'Contabilizada')
INSERT [dbo].[EstatusFactura] ([ID], [statu], [txtst]) VALUES (4, N'004', N'Pagada')
INSERT [dbo].[EstatusFactura] ([ID], [statu], [txtst]) VALUES (5, N'005', N'Rechazada')
INSERT [dbo].[EstatusFactura] ([ID], [statu], [txtst]) VALUES (6, N'006', N'Rechazada SAT')
