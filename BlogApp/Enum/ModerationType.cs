using System;
using System.ComponentModel;

namespace BlogApp.Enum
{
	public enum ModerationType
	{
		[Description("Political propaganada")]
		Political,
        [Description("Offensive language")]
        Language,
        [Description("Drug referances")]
        Drugs,
        [Description("Threatening Speech")]
        Threatening,
        [Description("Sexual content")]
        Sexual,
        [Description("Hate speech")]
        HateSpeech,
        [Description("Targeted shaming")]
        Shaming
		
	}
}

