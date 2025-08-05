/****** Object:  Table [dbo].[cuentaComercio]    Script Date: 03/07/2025 21:25:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[cuentaComercio](
	[idCuenta] [int] NOT NULL,
	[nombreUsuario] [nvarchar](20) NOT NULL,
	[contrasena] [nvarchar](20) NOT NULL,
	[cedulaJuridica] [nvarchar](20) NOT NULL,
	[telefono] [int] NOT NULL,
	[correoElectronico] [nvarchar](50) NOT NULL,
	[direccion] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idCuenta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[nombreUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


