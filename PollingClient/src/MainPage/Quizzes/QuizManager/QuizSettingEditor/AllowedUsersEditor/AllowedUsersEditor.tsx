import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal'
import styles from './AllowedUsersEditor.module.css'
import AddButton from './AddButton'
import AllowedUserAdd from './AllowedUserAdd'
import { useState, useEffect } from 'react'

interface AllowedUser {
    id: number,
    nickname: string,
} 

export default function AllowedUsersEditor(props: any) {
    const [allowedUsers, setAllowedUsers] = useState<AllowedUser[] | null>(null);
    const [showAddUser, setShowAddUser] = useState<boolean>(false);

    async function fetchUsers() {
        let response = await fetch(`/api/polls/${props.poll.id}/allowed-users`, {
            method: 'GET',
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json",
                "Authorization": "Bearer " + sessionStorage.getItem('token')
            },
        });
        if (response.ok) {
            let document: AllowedUser[] = await response.json() as AllowedUser[];
            setAllowedUsers(document);
        }
    }

    async function removeUser(allowedUser: AllowedUser) {
        let response = await fetch(`/api/polls/${props.poll.id}/allowed-users`, {
            method: 'DELETE',
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json",
                "Authorization": "Bearer " + sessionStorage.getItem('token')
            },
            body: JSON.stringify(allowedUser)
        });
        if (response.ok) {
            setAllowedUsers(allowedUsers?.filter((user: AllowedUser) => allowedUser.id != user.id ) ?? []);
        }
    }

    useEffect(() => { fetchUsers() }, []);

    return (
        <Modal show={props.show} fullscreen onHide={() => props?.setShow(false)}>
            <Modal.Header closeButton>
                <Modal.Title>Allowed users editor</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                {allowedUsers?.map((user) => (
                    <div className={styles.user} key={user.id}>
                        <span>{user.nickname}</span>
                        <Button onClick={(_) => removeUser(user)}>Remove</Button>
                    </div>
                ))}
            </Modal.Body>
            <div className={styles.addButtonWrapper}>
                <AddButton onClick={() => setShowAddUser(true)} />
            </div>
            <AllowedUserAdd
                poll={props.poll}
                show={showAddUser}
                update={fetchUsers}
                setShow={(isShow: boolean) => setShowAddUser(isShow)}
            />
        </Modal>
    );
}