USE PensaClub

DELETE FROM Places

SET IDENTITY_INSERT Places ON

INSERT INTO [dbo].[Places] ([Id], [Caption], [Lat], [Long], [Radius]) VALUES (1, N'Sofia', 42.698494, 23.3198532, 13)
INSERT INTO [dbo].[Places] ([Id], [Caption], [Lat], [Long], [Radius]) VALUES (2, N'Softuni', 42.6361668, 23.3698051, 18)
INSERT INTO [dbo].[Places] ([Id], [Caption], [Lat], [Long], [Radius]) VALUES (3, N'Burgas', 42.5266569, 27.287156, 13)

SET IDENTITY_INSERT Places OFF