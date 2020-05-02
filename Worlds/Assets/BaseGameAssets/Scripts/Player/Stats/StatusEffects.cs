using System.Collections.Generic;

namespace Worlds.Player
{
    public class StatusEffects
    {
        public List<Effect> effects = new List<Effect>();

        public void AddEffect(object sender, Status status, float duration = 1) =>
            effects.Add(new Effect(sender, status, duration));

        public void AddEffect(Effect effect) =>
            effects.Add(effect);

        public void UpdateEffects(float deltaTime)
        {
            if (effects == null || effects.Count == 0) return;

            for (int i = effects.Count - 1; i > 0; i--)
            {
                if (effects[i].duration <= 0)
                {
                    // Remove effect
                    effects.RemoveAt(i);
                }
                else
                {
                    // Lower the duration left on this effect
                    effects[i].ModDuration(-deltaTime);
                }
            }
        }
    }

    public enum Status
    {
        #region Exhaustion

        /// <summary>
        /// A character with Exaustion 1 will have disadvantage on ability checks.
        /// </summary>
        Exhaustion_1,

        /// <summary>
        /// A character with Exaustion 2 will have their speed halved.
        /// </summary>
        Exhaustion_2,

        /// <summary>
        ///  A character with Exaustion 3 will have their hit points halved.
        /// </summary>
        Exhaustion_3,

        /// <summary>
        ///  A character with Exhausiton 4 will be unable to move.
        /// </summary>
        Exhaustion_4,

        /// <summary>
        ///  A character with Exhausiton 5 will die.
        /// </summary>
        Exhaustion_5,

        #endregion Exhaustion

        /// <summary>
        /// An uncausious character cannot move or speak.
        /// This character will automatically fail all checks.
        /// Attacks against this character will automatically be critical attack.
        /// </summary>
        Unconsious,

        /// <summary>
        /// A character who is stunned can't move, but can speak faintly.
        /// This character will automatically fail all checks.
        /// Attacks against this character will automatically be critical attacks.
        /// </summary>
        Stunned,

        /// <summary>
        /// A restrained character cannot move, and attacks have disadvantage.
        /// Attacks against this character will automatically be critical attacks.
        /// </summary>
        Restrained,

        /// <summary>
        /// A poisoned creature has disadvantage, aswell as takes damage for the duration of the effect.
        /// </summary>
        Poisoned,

        /// <summary>
        /// A petrified character is transformed, along with any nonmagical object it is wearing or carrying, into a solid inanimate substance.
        /// A petrified character is incapacitated, can't move or speak, and is unaware of its surroundings.
        /// Attacks against this character will have advantage.
        /// Any checks this character attempts are automatic fails.
        /// The character is resistant to all damage.
        /// </summary>
        Petrified,

        /// <summary>
        /// A paralyzed character is incapacitated, and can't move or speak.
        /// Any attacks against this character will have advantage.
        /// Any attack that hits is an automatic critical hit.
        /// </summary>
        Paralyzed,

        /// <summary>
        /// An invisible character is impossible to see without the aid of magic or a Special sense.
        /// For the purpose of Hiding, the creature is heavily obscured.
        /// This character will have advantage on attacks, and any attacks against it will have disadvantage.
        /// </summary>
        Invisible,

        /// <summary>
        /// An incapacitated character can’t take Actions or reactions.
        /// </summary>
        Incapacitated,

        /// <summary>
        /// A character which is frightened has disadvantage on checks while it's source of fear is within sight.
        /// The character can’t willingly move closer to the source of its fear.
        /// </summary>
        Frightened,

        /// <summary>
        /// A deafened character can’t hear and automatically fails any ability check that requires hearing.
        /// </summary>
        Deafened,

        /// <summary>
        /// A character who is charmed cannot attack the charmer.
        /// </summary>
        Charmed,

        /// <summary>
        /// A character who is blinded cannot see anything, and will fail any checks that require sight.
        /// Attacks against this character will automatically be critical attacks.
        /// </summary>
        Blinded,

        /// <summary>
        /// A character which is silenced cannot speak, and any spoken checks will automatically fail.
        /// </summary>
        Silenced
    }

    public struct Effect
    {
        public object sender;
        public Status status;
        public float duration;

        public Effect(object sender, Status status, float duration = 1)
        {
            this.sender = sender;
            this.status = status;
            this.duration = duration;
        }

        public void ModDuration(float modAmount)
        {
            duration += modAmount;
        }
    }
}