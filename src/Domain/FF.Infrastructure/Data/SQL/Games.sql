INSERT INTO Games (Id, Title, Version, Author, Description, Year, 
FileName, FileUrl, PinmameRomId, CRC, 
FileNamePatched, FilePatchUrl, CRCPatched, PatchNotes,
IsDeleted, Created, GameSystem, GameType, IPDB, IsEnabled, Manufacturer, Players) VALUES 
('08a0c774-d73c-44eb-b5b6-9bf455f8e0d8', 'Test Table VP', '0.99', 'HorsePin', 'Testing ID for players before release', 2022,
NULL, '', NULL, 'CRC',
NULL, NULL, NULL, 'just added flip script', 
0, '2022-12-11 23:54:31.7364384', 1, 1, NULL, 1, NULL, 4),

('3d24da48-53d5-4ff3-96f0-7d41507dc39d', 'Sorcerer (Williams 1985)', '4.0.0', 'jpsalas', 'Based on the Williams table from 1985', 2022,
'Sorcerer (Williams 1985) v4.vpx', 'https://www.vpforums.org/index.php?app=downloads&showfile=13549', 'sorcr_l2', '759181AE', 
'Sorcerer (Williams 1985) v4-(Flips1.0).vpx', NULL, NULL, 'Just added flips scoring UI and script. Different game, has no last scores, see script',
 0,  '2022-12-11 23:54:31.7364613', 1, 194, 2242, 1, 'Williams', 4),

('5ea6aff2-75cf-4718-aa60-58b1d460954b', 'Bram Stoker''s Dracula (Williams 1993)', '2.0', 'Bigus1,ICPjuggla,Dozer316,JPSalas,francisco666,rom', 'This table from ICPjuggla and Dozer316 looks and plays great.', 2022, 
'Bram Stoker''s Dracula (Williams 1993)_Bigus(MOD)2.0.vpx', 'https://www.vpforums.org/index.php?app=downloads&showfile=15955', 'drac_l1', 'A0FF9E63', 
'Bram Stoker''s Dracula (Williams 1993)_Bigus(MOD)2.0.vpx-(Flips1.0).vpx', NULL, NULL, 'Just added flips scoring UI and script',
 0,  '2022-12-11 23:54:31.7364582', 1, 194, 3072, 1, 'Williams', 4),

('89559b4c-08e0-4f03-a74b-a6ba4c7414dd', 'Robocop (Data East 1989)', '2.1', 'Bigus1,ICPjuggla,Talantyyr,Dozer316,dark', 'This fun table is from ICPjuggla, Talantyyr, Dozer316 and dark', 2022, 
'Robocop (Data East 1989)_Bigus(MOD)2.1.vpx', 'https://www.vpforums.org/index.php?app=downloads&showfile=15421', 'robo_a34', 'FEB6FC87',
'Robocop (Data East 1989)_Bigus(MOD)2.1-(Flips1.0).vpx', NULL, NULL, 'Just added flips scoring UI and script',
 0,  '2022-12-11 23:54:31.7364557', 1, 194, 1976, 1, 'Data East', 4);
