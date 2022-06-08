using SFML.System;
using TCEngine;

namespace TCGame
{
    public class ExplosionComponent : BaseComponent
    {

        private bool m_DisableExplosion = false;

        public bool DisableExplosion
        {
            set => m_DisableExplosion = value;
        }

        public ExplosionComponent()
        {
        }

        public override void OnActorDestroyed()
        {
            base.OnActorDestroyed();

            if( m_DisableExplosion == false)
            {
                Actor explosionActor = new Actor("Explosion");
                TransformComponent transformComponent = explosionActor.AddComponent<TransformComponent>();
                transformComponent.Transform.Position = Owner.GetPosition();

                AnimatedSpriteComponent animatedSpriteComponent = explosionActor.AddComponent<AnimatedSpriteComponent>("Textures/FX/Explosion", 9u, 1u);
                animatedSpriteComponent.Loop = false;
                animatedSpriteComponent.FrameTime = 0.1f;

                TimerComponent timerComponent = explosionActor.AddComponent<TimerComponent>(animatedSpriteComponent.AnimationTime);
                timerComponent.OnTime += explosionActor.Destroy;
                explosionActor.AddComponent<ForwardMovementComponent>(new Vector2f(0.0f, 1.0f), 30.0f);
                TecnoCampusEngine.Get.Scene.CreateActor(explosionActor);
            }
        }

        public override EComponentUpdateCategory GetUpdateCategory()
        {
            return EComponentUpdateCategory.Update;
        }

        public override object Clone()
        {
            return new ExplosionComponent();
        }
    }
}
