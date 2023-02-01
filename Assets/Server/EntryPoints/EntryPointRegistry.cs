using System;
public class EntryPointRegistry
{
	private EntryPointBase[] m_entryPoints;

	public EntryPointRegistry()
	{
		m_entryPoints = new EntryPointBase[]
		{
			new InitializerEntryPoint(),
        };

		foreach (var entryPoint in m_entryPoints)
		{
			entryPoint.Init();
		}
	}

	public EntryPointBase[] EntryPoints
	{
		get
		{
			return m_entryPoints;
		}
	}
}

