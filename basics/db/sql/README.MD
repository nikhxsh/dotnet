ACID properties in the database system context are fundamentals, let’s just see what it means. It stands for Atomicity, Consistency, Integrity and Durability. 

Atomicity – It’s all or none. That means either complete transaction happens in a single shot or doesn’t happen at all.

Consistency – Database should be in a consistent state before and after the transaction.

Isolation – Multiple transactions should be able to run in parallel without any interference to each other (i.e., in isolation)

Durability – Transaction updates are durable (persistent) on successful completion, means updates are written to the disks, not ephemeral and should persist the system failure if any.



In a monolithic application, with use of relational DBs it’s comparatively easy to have ACID compliant transactions. There is a single database and transaction initiated to that DB can be committed or rolled back based on the outcome of the transaction. While it’s easy to work with, it brings lot of challenges like unavailability of a resource in high volume system. A record under transaction kept locked unless the transaction completes and that makes it unavailable for any other transaction which needs that record