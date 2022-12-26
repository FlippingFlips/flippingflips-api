-- UPDATE Json mappings
-- UPDATE PinMameRoms SET NvMapJson = '' WHERE Id = '';
UPDATE PinMameRoms SET NvMapJson = '{{
  "_notes": [
    "Compiled by Horsepin."
  ],
  "_copyright": "Copyright (C) 2022 https://github.com/horseyhorsey",
  "_license": "GNU Lesser General Public License v3.0",
  "_endian": "big",
  "_ramsize": 8238,
  "_roms": [
    "drac_l1"
  ],
  "_fileformat": 0.1,
  "_version": 0.1,
  "last_game": [
    {{
      "encoding": "bcd",
      "length": 5,
      "start": 5760
    }},
    {{
      "encoding": "bcd",
      "length": 5,
      "start": 5766
    }},
    {{
      "encoding": "bcd",
      "length": 5,
      "start": 5772
    }},
    {{
      "encoding": "bcd",
      "length": 5,
      "start": 5778
    }}
  ],
  "high_scores": [
    {{
      "label": "Greatest Vampire Hunter",
      "short_label": "GVP",
      "initials": {{"encoding":"ch","length":2,"start":8061}},
      "score": {{"encoding":"bcd","length":5,"start":7484}}
    }},
    {{
      "label": "Best Hunter #1",
      "short_label": "",
      "initials": {{"encoding":"ch","length":3,"start":7447}},
      "score": {{"encoding":"bcd","length":5,"start":7450}}
    }},
    {{
      "label": "Best Hunter #2",
      "initials": {{"encoding":"ch","length":3,"start":7455}},
      "score": {{"encoding":"bcd","length":5,"start":7458}}
    }},
    {{
      "label": "Best Hunter #3",
      "initials": {{"encoding":"ch","length":3,"start":7463}},
      "score": {{"encoding":"bcd","length":5,"start":7466}}
    }},
    {{
      "label": "Best Hunter #4",
      "initials": {{"encoding":"ch","length":3,"start":7471}},
      "score": {{"encoding":"bcd","length":5,"start":7474}}
    }}
  ],
  "mode_champions": [
    {{
      "label": "Loop Champion",
      "short_label": "LC",
      "initials": {{"encoding":"ch","length":3,"start":8061}},
      "score": {{"encoding":"bcd","length":2,"start":8067}}
    }}
  ],
  "adjustments": {{
    "A.1 Standard Adjustments": {{
      "01": {{"label":"Balls Per Game","short_label":"BPG","start":7045,"length":2,"encoding":"int","min":1,"max":10,"default":3}},
      "02": {{"label":"Tilt Warnings","short_label":"TW","start":7047,"length":2,"encoding":"int","min":1,"max":10,"default":3}},
      "03": {{"label":"Max E.B Count","short_label":"MEB","start":7049,"length":2,"encoding":"int","min":0,"max":10,"default":4}},
      "04": {{"label":"Max E.B Per B.I.P","start":7051,"length":2,"encoding":"int","min":0,"max":10,"default":0,"special_values":{{"0":"OFF"}}}},
      "05": {{"label":"Replay System","start":7054,"length":0,"encoding":"enum","default":0,"values":["Auto %"," Fixed"]}},
      "06": {{"label":"Replay Percent","start":7055,"length":2,"encoding":"int","min":5,"max":50,"default":10}},
      "07": {{"label":"Replay Start","start":7057,"length":2,"encoding":"bcd","min":5,"max":305,"default":200,"multiple_of":3,"suffix":"M"}},
      "08": {{"label":"Replay Levels","start":7059,"length":2,"encoding":"int","min":1,"max":4,"default":1}},
      "13": {{"label":"Replay Boost","start":7069,"length":2,"encoding":"bcd","min":0,"max":500,"default":50,"multiple_of":50,"suffix":"M","special_values":{{"0":"OFF"}}}},
      "14": {{"label":"Replay Award","start":7072,"length":0,"encoding":"enum","default":0,"values":["Credit","Ticket","Ball","Audit"]}},
      "15": {{"label":"Special Award","start":7074,"length":0,"encoding":"enum","default":0,"values":["Credit","Ticket","Ball","Points"]}},
      "16": {{"label":"Match Award","start":7076,"length":0,"encoding":"enum","default":0,"values":["Credit","Ticket"]}},
      "17": {{"label":"Ex. Ball Ticket","start":7078,"length":0,"encoding":"enum","default":0,"values":["NO","YES"]}},
      "18": {{"label":"Max. Tick,/Player","start":7079,"length":2,"encoding":"int","min":0,"max":50,"default":10,"suffix":"%","special_values":{{"0":"OFF"}}}},
      "19": {{"label":"Match Feature","start":7081,"length":2,"encoding":"int","min":0,"max":50,"default":10,"suffix":"%","values":["0","OFF"]}},
      "20": {{"label":"Custom Message","start":7084,"length":1,"encoding":"enum","default":1,"values":["OFF","ON"]}},
      "21": {{"label":"Language","start":7086,"length":0,"encoding":"enum","default":0,"values":["English","French","German"]}},
      "22": {{"label":"Clock Style","start":7088,"length":0,"encoding":"enum","default":0,"values":["AM/PM","24 Hour"]}},
      "23": {{"label":"Date Style","start":7090,"length":0,"encoding":"enum","default":0,"values":["Mmm. DD YY"," DD Mmm. YY"]}},
      "24": {{"label":"Show Time + Date","start":7092,"length":0,"encoding":"enum","default":0,"values":["NO","YES"]}},
      "25": {{"label":"Allow Dm. Illum","start":7094,"length":0,"encoding":"enum","default":1,"values":["NO","YES"]}},
      "26": {{"label":"Tournament Play","start":7096,"length":0,"encoding":"enum","default":0,"values":["NO","YES"]}},
      "27": {{"label":"Euro. Scr Format","start":7098,"length":0,"encoding":"enum","default":0,"values":["NO","YES"]}},
      "28": {{"label":"Min. Vol. Override","start":7100,"length":0,"encoding":"enum","default":0,"values":["NO","YES"]}},
      "29": {{"label":"GI Power Saver","start":7101,"length":2,"encoding":"int","min":1,"max":60,"default":15,"suffix":" MINUTES","special_values":{{"1":"OFF"}}}},
      "30": {{"label":"Power Save Level","start":7103,"length":2,"encoding":"int","min":4,"max":7,"default":5}},
      "31": {{"label":"Ticket Exp. Board","start":7106,"length":0,"encoding":"enum","default":0,"values":["NO","YES"]}},
      "32": {{"label":"No Bonus Flips","start":7108,"length":0,"encoding":"enum","default":0,"values":["NO","YES"]}},
      "33": {{"label":"Game Restart","start":7110,"length":0,"encoding":"enum","default":1,"values":["NEVER","SLOW","INSTANTLY"]}}
    }},
    "A.2 Feature Adjustments": {{
      "01": {{"label":"Ball  Time","short_label":"BT","start":7203,"length":2,"encoding":"int","min":0,"max":99,"default":35,"suffix":" Sec","special_values":{{"0":"OFF"}}}},
      "02": {{"label":"Auto. %Special&Match","start":7206,"length":0,"encoding":"enum","default":1,"values":["NO","YES"]}},
      "03": {{"label":"Ball Saver","short_label":"BS","start":7207,"length":2,"encoding":"int","min":0,"max":15,"default":2,"suffix":" Sec","special_values":{{"0":"OFF"}}}},
      "04": {{"label":"Auto. Ex.Ball","start":7209,"length":2,"encoding":"int","min":0,"max":99,"default":20,"suffix":" %","special_values":{{"0":"OFF"}}}},
      "05": {{"label":"1st ExB. Lit At","start":7211,"length":2,"encoding":"int","min":1,"max":99,"default":8,"suffix":" Loops"}},
      "06": {{"label":"Next ExB. Lit At","start":7213,"length":2,"encoding":"int","min":1,"max":99,"default":16,"suffix":" Loops"}},
      "07": {{"label":"V.Mode ExBall At","start":7215,"length":2,"encoding":"int","min":1,"max":99,"default":2,"suffix":" Waves"}},
      "08": {{"label":"Volume Change","start":7218,"length":0,"encoding":"enum","default":1,"values":["NO","YES"]}},
      "09": {{"label":"Pre-lite V.Mode","start":7219,"length":2,"encoding":"int","min":0,"max":5,"default":0,"suffix":" Lamp","special_values":{{"0":"NONE"}}}},
      "10": {{"label":"V.Mode Award","start":7221,"length":2,"encoding":"int","min":0,"max":4,"default":0,"suffix":" Lamp","special_values":{{"0":"NONE"}}}},
      "11": {{"label":"Bonus 'X' Memory","start":7224,"length":0,"encoding":"enum","default":1,"values":["NO","YES"]}},
      "12": {{"label":"Castle Lock Time","start":7225,"length":2,"encoding":"int","min":1,"max":99,"default":15}},
      "13": {{"label":"Castle Jackpot","start":7227,"length":2,"encoding":"int","min":0,"max":120,"default":20,"suffix":" Sec","special_values":{{"0":"OFF"}}}},
      "14": {{"label":"Jet Timer","start":7229,"length":2,"encoding":"int","min":0,"max":120,"default":10,"special_values":{{"0":"OFF"}}}},
      "15": {{"label":"Pre-lite Coffin","start":7231,"length":2,"encoding":"int","min":0,"max":4,"default":0,"suffix":" Lamps","special_values":{{"0":"NONE"}}}},
      "16": {{"label":"Coffin Ramp Time","start":7233,"length":2,"encoding":"int","min":0,"max":120,"default":15,"suffix":" Sec","special_values":{{"0":"OFF"}}}},
      "17": {{"label":"Left Ramp Time","start":7235,"length":2,"encoding":"int","min":0,"max":120,"default":10,"suffix":" Sec","special_values":{{"0":"OFF"}}}},
      "18": {{"label":"Mystery Time","start":7237,"length":2,"encoding":"int","min":0,"max":120,"default":5,"suffix":" Sec","special_values":{{"0":"OFF"}}}},
      "19": {{"label":"Mystery Pre-Lit","start":7239,"length":0,"encoding":"enum","default":0,"values":["Game Start","Not Lit","Ball Start"]}},
      "20": {{"label":"Myst:Castle Lock","start":7241,"length":2,"encoding":"int","min":0,"max":99,"default":10,"suffix":" %","special_values":{{"0":"OFF"}}}},
      "21": {{"label":"Myst:Coffin Lock","start":7243,"length":2,"encoding":"int","min":0,"max":99,"default":10,"suffix":" %","special_values":{{"0":"OFF"}}}},
      "22": {{"label":"Myst:V.Mode Lit","start":7245,"length":2,"encoding":"int","min":0,"max":99,"default":10,"suffix":" %","special_values":{{"0":"OFF"}}}},
      "23": {{"label":"Myst:Bonus","start":7247,"length":2,"encoding":"int","min":0,"max":99,"default":25,"suffix":" %","special_values":{{"0":"OFF"}}}},
      "24": {{"label":"Video Mode Diff.","start":7250,"length":0,"encoding":"enum","default":2,"values":["Ex. Easy","Easy","Medium","Hard","Ex.Hard"]}},
      "25": {{"label":"1st Loop Bonus","start":7251,"length":2,"encoding":"int","min":1,"max":99,"default":6,"suffix":" Loop"}},
      "26": {{"label":"Other Loop Bonus","start":7253,"length":2,"encoding":"int","min":1,"max":99,"default":6,"suffix":" Loop"}},
      "27": {{"label":"1st Mist M.B. Lit","start":7255,"length":2,"encoding":"int","min":0,"max":99,"default":5,"suffix":" Loop","special_values":{{"0":"NONE"}}}},
      "28": {{"label":"Other Mist M.B","start":7257,"length":2,"encoding":"int","min":5,"max":99,"default":10,"suffix":" Loop"}},
      "29": {{"label":"1st Bats","short_label":"","start":7259,"length":2,"encoding":"int","min":0,"max":99,"default":3,"suffix":" Loop","special_values":{{"0":"NONE"}}}},
      "30": {{"label":"Other Bats","start":7261,"length":2,"encoding":"int","min":2,"max":99,"default":10,"suffix":" Loop"}},
      "31": {{"label":"Bats Time","start":7263,"length":2,"encoding":"int","min":0,"max":45,"default":15,"suffix":" Sec","special_values":{{"0":"OFF"}}}},
      "32": {{"label":"Number of Bats","start":7265,"length":2,"encoding":"int","min":5,"max":30,"default":15}},
      "33": {{"label":"Outlane Lit at","start":7267,"length":2,"encoding":"int","min":1,"max":99,"default":15,"suffix":" Altar"}},
      "34": {{"label":"1st Rats At","short_label":"","start":7269,"length":2,"encoding":"int","min":1,"max":99,"default":3,"suffix":" Altar"}},
      "35": {{"label":"Next Rats At","start":7271,"length":2,"encoding":"int","min":1,"max":99,"default":5,"suffix":" Altar"}},
      "36": {{"label":"Number of Rats","start":7273,"length":2,"encoding":"int","min":5,"max":99,"default":15}},
      "37": {{"label":"Rats Timer","start":7275,"length":2,"encoding":"int","min":1,"max":120,"default":15,"suffix":" Sec"}},
      "38": {{"label":"Timed Plunger","start":7277,"length":2,"encoding":"int","min":0,"max":120,"default":0,"multiple_of":5,"suffix":" Sec","special_values":{{"0":"OFF"}}}},
      "39": {{"label":"Flipped Plunger","start":7280,"length":0,"encoding":"enum","default":0,"values":["NO","YES"]}},
      "40": {{"label":"Fancy A. Mode G.I","start":7282,"length":0,"encoding":"enum","default":1,"values":["NO","YES"]}},
      "41": {{"label":"Attr. Mode Sounds","start":7284,"length":0,"encoding":"enum","default":0,"values":["NONE"," SOME"," ALOT"]}},
      "42": {{"label":"Magnet","start":7286,"length":0,"encoding":"enum","default":1,"values":["Disabled","Enabled"]}},
      "43": {{"label":"Family Mode","start":7288,"length":0,"encoding":"enum","default":0,"values":["NO","YES"]}},
      "44": {{"label":"Game Over Unlock","start":7290,"length":0,"encoding":"enum","default":0,"values":["NO","YES"]}}
    }}
  }}
}}' WHERE Id = 'drac_l1';


UPDATE PinMameRoms SET NvMapJson = '{{
	"_notes": ["Compiled by Horsepin.", "There are no saved last scores and no initials. You still need to add initials into the entry otherwise you cannot print the results."],
	"_copyright": "Copyright (C) 2017 by Dave Horse <horse@horsesoft.uk>",
	"_license": "GNU Lesser General Public License v3.0",
	"_endian": "big",
	"_roms": ["sorcr_l1","sorcr_l2"],
	"_fileformat": 0.1,
	"_version": 0.2,	
	"high_scores": [
		{{	"label": "First Place", "short_label": "#1",
			"initials": {{"start": "0x737"}},
			"score": {{"start": 1844, "encoding": "bcd", "packed" : true, "length": 4, "scale" : 1}}
		}},
		{{
			"label": "2nd Place", "short_label": "#2",
			"initials": {{"start": "0x737", "encoding": "ch", "length": 3}},
			"score": {{ "start": 1848, "encoding": "bcd", "packed": true, "length": 4,"scale": 1
			}}
		}},
		{{
			"label": "3rd Place", "short_label": "#3", 
			"initials": {{"start": "0x737","encoding": "ch","length": 3
			}},
			"score": {{"start": 1852,"encoding": "bcd","packed": true,"length": 4,"scale": 1
			}}
		}},
		{{
			"label": "4th Place","short_label": "#4",
			"initials": {{"start": "0x737","encoding": "ch","length": 3}},
			"score": {{"start": 1856,"encoding": "bcd","packed": true,"length": 4,"scale": 1
			}}
		}}		
	]
}}
' WHERE Id = 'sorcr_l2';



UPDATE PinMameRoms SET NvMapJson = '{{
  "_notes": [ "Compiled by Horsepin." ],
  "_copyright": "Copyright (C) 2016 by Horsepin",
  "_license": "GNU Lesser General Public License v3.0",
  "_endian": "big",
  "_roms": [ "robo_a34" ],
  "_fileformat": 0.1,
  "_version": 0.1,
  "last_game": [ 
  	{{ "start": 423, "encoding": "bcd", "length": 7, "packed": false }},
    {{ "start": 589, "encoding": "bcd", "length": 7, "packed": false }},
    {{ "start": 755,"encoding": "bcd", "length": 7, "packed": false }},
    {{ "start": 921,"encoding": "bcd", "length": 7, "packed": false }}
	],
  "high_scores": [
    {{ "label": "Commander", "short_label": "GC", "initials": 
    	{{ "start": "0x1C67", "encoding": "ch", "length": 3 }}, 
    	"score":  
    	{{ "start": "0x1C60", "encoding": "bcd", "length": 7, "packed": false }}
    }},
    {{ "label": "1st Place", "short_label": "1st", "initials": {{
        "start": "0x1C73", "encoding": "ch", "length": 3 }},
      "score": 
      {{ "start": "0x1C6C", "encoding": "bcd", "length": 7, "packed": false }}
    }},
    {{ "label": "2nd Place", "short_label": "2nd", "initials": {{
    	"start": "0x1C7F", "encoding": "ch", "length": 3 
    }},
      "score": 
      {{ "start": "0x1C78", "encoding": "bcd", "length": 7, "packed": false }}
    }},
    {{ "label": "3rd Place", "short_label": "3rd", "initials": {{
        "start": "0x1C8B", "encoding": "ch", "length": 3
	}},
      "score": 
	{{ "start": "0x1C84", "encoding": "bcd", "length": 7, "packed": false }}
    }}
	],
  "mode_champions": [
    {{
      "label": "Jump Master", "short_label": "Jump Master",
      "initials": {{ "start": "0x1C9F", "encoding": "ch", "length": 3 }},
      "score": {{ "start": "0x1C98", "encoding": "int", "length": 1, "suffix": " Jumps"
      }}
    }}
  ],
  "adjustments": {{
    "Feature Adjustments": {{
      "01": {{
        "label": "Replay Manual", "start": "0x1F03", "encoding": "int",
        "min": 0, "max": 50, "default": 12, "special_values": {{ "0": "FIXED" }}
      }},
      "02": {{
        "label": "Start Replay", "start": "0x1F05", "encoding": "int",
        "min": 0, "max": 9900000, "default": 2400000, "scale": 100000
      }},
      "03": {{ 
      	"label": "Levels Replay", "start": "0x1F07", "encoding": "int",
        "min": 1, "max": 4, "default": 1
      }},
      "06": {{
        "label": "Easy Rules", "start": "0x1F0D", "encoding": "enum",
        "default": 0, "values": [ "OFF", "ON" ]
      }},
      "07": {{
        "label": "Factory Rules", "start": "0x1F0F", "encoding": "enum",
        "values": [ "OFF", "ON" ]
      }},
      "08": {{
        "label": "Hard Rules", "start": "0x1F11", "encoding": "enum",
        "values": [ "OFF", "ON" ]
      }},
      "09": {{
        "label": "Novelty Rules", "start": "0x1F13", "encoding": "enum",
        "values": [ "OFF", "ON" ]
      }},
      "10": {{
        "label": "5 Ball Rules", "start": "0x1F15", "encoding": "enum",
        "values": [ "OFF", "ON" ]
      }},
      "11": {{
        "label": "Add A Ball Rules", "start": "0x1F17", "encoding": "enum",
        "values": [ "OFF", "ON" ]
      }},
      "12": {{
        "label": "Coins", "start": "0x1F19", "encoding": "enum",
        "values": [ "OFF", "ON" ]
      }},
      "13": {{
        "label": "Avg Ball Time", "start": "0x1F1B", "encoding": "enum",
        "values": [ "OFF", "ON" ]
      }},
      "14": {{
        "label": "Coins Reset", "start": "0x1F1D", "encoding": "enum",
        "values": [ "OFF", "ON" ]
      }},
      "15": {{
        "label": "Audit Reset", "start": "0x1F1F","encoding": "enum",
        "values": [ "OFF", "ON" ]
      }},
      "16": {{
        "label": "Hi Score Reset", "start": "0x1F21", "encoding": "enum",
        "values": [ "OFF", "ON" ]
      }},
      "17": {{
        "label": "Expand Adjustment", "start": "0x1F23", "encoding": "enum",
        "values": [ "OFF", "ON" ]
      }},
      "18": {{
        "label": "Award Replay", "start": "0x1F25", "encoding": "enum",
        "values": [ "Credits", "Extra Ball", "Audit", "Ticket" ]
      }},
      "19": {{
        "label": "Award Replay", "start": "0x1F27", "encoding": "enum",
        "values": [ "Credits", "Extra Ball", "Score", "Ticket" ]
      }},
      "20": {{
        "label": "Match Feature", "start": "0x1F29", "encoding": "enum",
        "values": [ "OFF", "ON" ]
      }},
      "21": {{
        "label": "Balls Game", "start": "0x1F2B", "encoding": "int",
        "min": 2, "max": 9, "default": 3
      }},
      "22": {{
        "label": "Warning Tilt", "start": "0x1F2D", "encoding": "int",
        "min": 0, "max": 5, "default": 1
      }},
      "23": {{
        "label": "Extra Balls", "start": "0x1F2F", "encoding": "int",
        "min": 0, "max": 5, "default": 3
      }},
      "24": {{
        "label": "Credits Limit", "start": "0x1F31", "encoding": "int",
        "min": 2, "max": 99, "default": 30
      }},
      "25": {{
        "label": "High Score Feature",  "start": "0x1F33", "encoding": "enum",
        "default": 1, "values": [ "OFF", "ON" ]
      }},
      "26": {{
        "label": "Credits HiScore 1", "start": "0x1F35", "encoding": "int",
        "min": 0, "max": 9, "default": 3
      }},
      "27": {{
        "label": "Credits HiScore 2", "start": "0x1F37", "encoding": "int",
        "min": 0, "max": 9, "default": 0
      }},
      "28": {{
        "label": "Credits HiScore 3", "start": "0x1F39", "encoding": "int",
        "min": 0, "max": 9, "default": 0
      }},
      "29": {{
        "label": "Credits HiScore 4", "start": "0x1F3B", "encoding": "int",
        "min": 0, "max": 9, "default": 0
      }},
      "30": {{
        "label": "Backup HiScore 1", "start": "0x1F3D", "encoding": "int",
        "min": 0, "max": 9900000, "default": 4000000, "scale": 100000
      }},
      "31": {{
        "label": "Backup HiScore 2", "start": "0x1F3F", "encoding": "int",
        "min": 0, "max": 9900000, "default": 3500000, "scale": 100000
      }},
      "32": {{
        "label": "Backup HiScore 3", "start": "0x1F41", "encoding": "int",
        "min": 0, "max": 9900000, "default": 3000000, "scale": 100000
      }},
      "33": {{
        "label": "Backup HiScore 4", "start": "0x1F43", "encoding": "int",
        "min": 0, "max": 9900000, "default": 2500000, "scale": 100000
      }},
      "34": {{
        "label": "Reset Every", "start": "0x1F45", "encoding": "int",
        "min": 100, "max": 900, "default": 700, "scale": 100
      }},
      "35": {{
        "label": "Free Play", "start": "0x1F47", "encoding": "enum",
        "values": [ "OFF", "ON" ]
      }},
      "41": {{
        "label": "Message Custom", "start": "0x1F53", "encoding": "enum",
        "values": [ "OFF", "ON", "CHANGE" ]
      }},
      "42": {{
        "label": "Attract Music", "start": "0x1F55", "encoding": "enum",
        "values": [ "OFF", "ON", "OFTEN" ]
      }},
      "43": {{
        "label": "Buy In Feature", "start": "0x1F57", "encoding": "enum",
        "values": [ "OFF", "ON" ]
      }},
      "44": {{
        "label": "Flash Lamp", "start": "0x1F59", "encoding": "enum",
        "values": [ "OFF", "ON" ]
      }},
      "45": {{
        "label": "Stand Up Seq", "start": "0x1F5B", "encoding": "enum",
        "values": [ "EASY", "HARD" ]
      }},
      "46": {{
        "label": "Spinner Lamp", "start": "0x1F5D", "encoding": "enum",
        "values": [ "EASY", "HARD" ]
      }},
      "47": {{
        "label": "Spot StandUp", "start": "0x1F5F", "encoding": "enum",
        "values": [ "EASY", "HARD" ]
      }},
      "48": {{
        "label": "Target Practice", "start": "0x1F61", "encoding": "enum",
        "values": [ "EASY", "HARD" ]
      }},
      "49": {{
        "label": "1 Meg Target", "start": "0x1F63", "encoding": "int",
        "min": 1, "max": 8, "default": 4
      }},
      "50": {{
        "label": "Laser Kick On", "start": "0x1F65", "encoding": "enum",
        "values": [ "EASY", "HARD" ]
      }},
      "51": {{
        "label": "Special Target", "start": "0x1F67", "encoding": "int",
        "min": 1, "max": 8, "default": 5
      }},
      "52": {{
        "label": "2X Scores Time", "start": "0x1F69", "encoding": "int",
        "min": 5, "max": 25, "default": 10
      }},
      "53": {{
        "label": "All Lit Jumps", "start": "0x1F6B", "encoding": "int",
        "min": 5, "max": 25, "default": 5
      }},
      "54": {{
        "label": "Scanning Extra Ball", "start": "0x1F6D", "encoding": "int",
        "min": 5, "max": 50, "default": 25
      }},
      "55": {{
        "label": "Factory Restore", "start": "0x1F6F", "encoding": "enum",
        "values": [ "OFF", "ON" ]
      }}
    }}
  }}
}}
' WHERE Id = 'robo_a34';