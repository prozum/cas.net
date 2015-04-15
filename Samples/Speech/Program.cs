using System;

namespace SampleSynthesis
{
	class Program
	{
		static void Main(string[] args)
		{

			var speechSynthesizer = new AVSpeechSynthesizer ();
			var speechUtterance =
				new AVSpeechUtterance ("Shall we play a game?");
			speechSynthesizer.SpeakUtterance (speechUtterance);
		}
	}
}