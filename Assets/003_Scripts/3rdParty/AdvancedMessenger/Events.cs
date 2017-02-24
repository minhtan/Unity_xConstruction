public class Events {
	public enum Input{
		PRESSED,
		RELEASED,
		HOLD
	}

	public enum Buttons{
		SUSPENSION,
		RAIL,
		DELETE,
		PLAY,
		RESET,
		PLAY_AGAIN,
		NEXT_LEVEL
	}

	public enum Game{
		PART_CHANGED,
		TIME_CHANGED,
		WIN,
		LOSE
	}
}
