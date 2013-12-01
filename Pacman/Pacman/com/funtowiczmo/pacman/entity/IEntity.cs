using Pacman.com.funtowiczmo.pacman.entity.signal;
using System;
namespace Pacman.com.funtowiczmo.pacman.entity
{
    public interface IEntity : IObservable<EntitySignal>
    {
		int GetID();
        string[] GetDefaultSkins();
	}

}
