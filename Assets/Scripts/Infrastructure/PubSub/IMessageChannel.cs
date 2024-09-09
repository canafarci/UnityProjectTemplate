using System;

namespace ProjectTemplate.Infrastructure.PubSub
{
	public interface IPublisher<T>
	{
		void Publish(T message);
	}

	public interface ISubscriber<T>
	{
		IDisposable Subscribe(Action<T> handler);
		void Unsubscribe(Action<T> handler);
	}

	public interface IMessageChannel<T> : IPublisher<T>, ISubscriber<T>, IDisposable
	{
		bool isDisposed { get; }
	}

	public interface IBufferedMessageChannel<T> : IMessageChannel<T>
	{
		bool hasBufferedMessage { get; }
		T bufferedMessage { get; }
	}
}