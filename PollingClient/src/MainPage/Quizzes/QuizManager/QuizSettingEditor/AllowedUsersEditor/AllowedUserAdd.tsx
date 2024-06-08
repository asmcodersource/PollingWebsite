import Modal from 'react-bootstrap/Modal'
import Button from 'react-bootstrap/Button'
import './AllowedUserAdd.module.css'
import {useState} from 'react'

export default function AllowedUserAdd(props: any) {
    const [allowedUser, setAllowedUser] = useState<string>("");
    const [errorMsg, _] = useState<string>();

    async function addUser(nickname: string) {
        let response = await fetch(`/api/polls/${props.poll.id}/allowed-users`, {
            method: 'POST',
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json",
                "Authorization": "Bearer " + sessionStorage.getItem('token')
            },
            body: JSON.stringify({ nickname: nickname })
        });
        if (response.ok) {
            props.update();
            props.setShow(false);
        }
    }

    return (
        <Modal show={props.show} fullscreen onHide={() => props?.setShow(false)}>
            <Modal.Header closeButton>
                <Modal.Title>Add allowed user</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <div className="limiter-container-wrapper ">
                    <div className="limiter-container create-new-item-dialog">
                        <label>User nickname</label><br />
                        <input
                            onChange={(e) => setAllowedUser(e.target.value) }
                            value={allowedUser}
                            id="field-name"
                            placeholder="nickname of user"
                        >
                        </input>
                        <br />
                        <p className="text-secondary text-justify">The field name field is needed for internal data processing needs (in the future it can be used to export data from the site). The description field is the message that will be displayed before a specific input/selection field. The object type determines what type of question will be created. After creation, you can edit the created question.</p>
                        <p className="error-box">
                            {errorMsg}
                        </p>
                        <div className="buttons-wrapper">
                            <Button onClick={(_) => addUser(allowedUser)} className="create" variant="primary">Add</Button>
                        </div>
                    </div>
                </div>
            </Modal.Body>
        </Modal>
    )
}