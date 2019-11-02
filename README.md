# McGill Hackathon 2019

Repository for McGill Hackathon 2019.

## Requirements

- [Unity](https://unity.com/)
- [Ml-Agents Toolkit](https://github.com/Unity-Technologies/ml-agents)

## Installation

1. Install Unity.
2. Install Anaconda and setup for Ml-Agents toolkit.
3. Git clone or download the Ml-Agents toolkit.
4. Git clone or download this repository.

## Config File

You'll need to set up a config under `/config`. I made a custom yml in the repository called mh_training_config.yml. You'll also need to put a new curriculum under `/config/curricula`. Copy the `mh` folder in the repo.


## Training the Agents

Using the Anaconda Prompt

```
conda activate ml-agents
```

Then, cd to the directory where you cloned ml-agents.

```
mlagents-learn <trainer-config-file> --run-id=<run-identifier> --train
```