Indoors Temps

This is simple mod that address TLD issue of that indoors are not reposnding to external temperature. Idea is based of what Relentless Night mod does, but it's much simpler and should work with any other mod that control temperature.

In vanilla, indoor areas (those that do scene change, so like Grey Mother House) have static temperature of -5, regardless of what is outside.
Other shelters add flat bonus:
* snow shelter +15C
* semi indoor +8C
* cars +5C

This mods chnage this in two ways.
1) Indoor scenes, will set temperature in between Max temp and basetemp, base of insulation factor (Settings). Setting it insulation to 1, means that Max temp will be used, setting it to 0 means Base temp will be used.
2) all others do not provide this flat bonus but add bonus base on same formula as above but each of them is capped with max bonus of vanilla setting.

In laymans terms, this means:
* any shellter is usually colder than vanilla.
* no more scenario when getting into back cave or car, will result higher temp than fully build house
* temperature calculations are not affected by wind and blizzard (not full indoor scense will protectu you from wind shill, but their bonus will same as if blizard is or is not (so colder))

Upper boundaries ratio - this settings will drastically reduce insulation efficency.
Having it at 0, means that insualation calculation, upper boundary (as, if insulation is set to 1) will be 0C. Having it at 1, means it will be Maxiumum possible temperature of a day, which as game progress is usually below -10C (other mods can affect this).

