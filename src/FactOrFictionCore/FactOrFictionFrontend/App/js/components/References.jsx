import * as React from 'react';
import PropTypes from 'prop-types';
import shortid from 'shortid';

export default class References extends React.Component {
    static propTypes = {
        references: PropTypes.array.isRequired,
        cleanLink: PropTypes.func.isRequired
    }

    render() {
        const { references } = this.props;
        
        return (
            references
                .map(ref => this.formatReference(ref))
                .map(ref => (
                    <tr key={shortid.generate()} style={{width: "100%"}}>
                        <td>
                            <a href={ref.link} target="_blank" > {ref.display} </a>
                        </td>
                        <td>
                            {ref.bias}
                        </td>
                    </tr>
                )   
            )        
        );
    }

    formatReference = ref => {
        const { cleanLink } = this.props;
        
        return {
            "link": ref.link,
            "display": cleanLink(ref.link),
            "bias": (ref.hasOwnProperty('bias') && ref.bias !== null) 
                ? ref.bias.biasType
                : null
        };
    };
}