import styles from './AddButton.module.css'

export default function AddButton(props: any) {
    return (
        <div className={styles.addButton} onClick={ props.onClick }>
            <span>+</span>
        </div>
    )
}