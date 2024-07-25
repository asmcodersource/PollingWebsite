import React, { useState } from 'react';
import { Modal, Button, Form } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';

const CreateQuizDialog = (props:any) => {
    const [name, setName] = useState('');

    const handleCreate = async () => {
        let response = await fetch(`/api/polls`, {
            method: "POST",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json",
                "Authorization": "Bearer " + sessionStorage.getItem('token')
            },
            body: JSON.stringify(
                {
                    name: name,
                    description: "",
                    type: "OnlyOwner",
                    access: "Public"
                }
            )
        });
        props.hide();
    };

    return (
        <>
            <Modal show={props.show} fullscreen onHide={props.hide}>
                <Modal.Header closeButton>
                    <Modal.Title>Create new quiz</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>
                        <Form.Group controlId="formObjectName">
                            <Form.Label>Name</Form.Label>
                            <Form.Control
                                type="text"
                                placeholder="Enter name"
                                value={name}
                                onChange={(e) => setName(e.target.value)}
                                required
                            />
                        </Form.Group>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={props.hide}>
                        Cancel
                    </Button>
                    <Button variant="primary" onClick={() => handleCreate()}>
                        Create
                    </Button>
                </Modal.Footer>
            </Modal>
        </>
    );
};

export default CreateQuizDialog;
